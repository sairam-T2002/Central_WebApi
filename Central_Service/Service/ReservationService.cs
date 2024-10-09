using Central_Service.Core;
using Central_Service.Interface;
using Microsoft.Extensions.Logging;
using Repository_DAL_.Model;
using Repository_DAL_;
using Central_Service.DTO;
using ExtensionMethods;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Central_Service.Service
{
    public class ReservationService : ServiceBase, IReservationService
    {
        private readonly IRepository<Reservation> _reservationRepository;
        private readonly IRepository<ApiLog> _logService;
        private readonly EFContext _context;

        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

        public ReservationService( ILogger<ReservationService> logger, IServiceProvider serviceProvider, IRepository<Reservation> reservationRepository, IRepository<ApiLog> logService, EFContext context ) : base(logger, serviceProvider)
        {
            _reservationRepository = reservationRepository;
            _context = context;
            _logService = logService;
        }

        public async Task<bool> ReserveCart( List<ProductDto> cart, int userId )
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                if (!await UpdateStockCount(cart))
                {
                    await transaction.RollbackAsync();
                    return false;
                }
                string reservationId = $"Reservation_{Guid.NewGuid()}";
                var newReservation = new Reservation
                {
                    Reservation_Id = reservationId,
                    Cart = cart.JSONStringify(),
                    CreatedTime = DateTime.UtcNow,
                    ExpireTime = DateTime.UtcNow.AddMinutes(10),
                    IsExpired = false,
                    Id = userId
                };
                await _reservationRepository.Add(newReservation);
                await transaction.CommitAsync();
                _ = Task.Run(() => MonitorReservationStatus(reservationId, CancellationToken.None));
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                await LogAsync(nameof(ReserveCart), LogUtil.Exception, ex.JSONStringify(), ex.Message);
                return false;
            }
        }


        private async Task<bool> UpdateStockCount( List<ProductDto> cart )
        {
            await _semaphore.WaitAsync();
            try
            {
                if (cart == null || !cart.Any())
                    return true;
                var productIds = cart.Select(item => item.Product_Id).ToList();
                var products = await _context.Products.Where(p => productIds.Contains(p.Product_Id)).ToListAsync();
                foreach (var item in cart)
                {
                    var product = products.FirstOrDefault(p => p.Product_Id == item.Product_Id);
                    if (product == null || product.StockCount < item.Quantity)
                    {
                        return false;
                    }
                    product.StockCount -= item.Quantity;
                }
                try
                {
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    await LogAsync(nameof(UpdateStockCount), LogUtil.Exception, ex.JSONStringify(), "Concurrency conflict occurred while updating stock count.");
                    return false;
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private async Task MonitorReservationStatus( string reservationId, CancellationToken cancellationToken )
        {
            try
            {
                using var scope = CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<EFContext>();
                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(TimeSpan.FromMinutes(3), cancellationToken);
                    var reservation = await GetReservationById(reservationId, context);
                    if (reservation == null || reservation.ConfirmedTime != null)
                    {
                        break;
                    }

                    if (IsReservationExpired(reservation))
                    {
                        await HandleReservationExpiration(reservation, context);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                await LogAsync(nameof(MonitorReservationStatus), LogUtil.Exception, ex.JSONStringify(), ex.Message);
            }
        }

        private bool IsReservationExpired( Reservation reservation )
        {
            return DateTime.UtcNow > reservation.ExpireTime;
        }

        private async Task HandleReservationExpiration( Reservation reservation, EFContext context )
        {
            await _semaphore.WaitAsync();
            try
            {
                List<ProductDto> cart = reservation.Cart.JSONParse<List<ProductDto>>();
                using var transaction = await context.Database.BeginTransactionAsync();
                try
                {
                    reservation.IsExpired = true;
                    context.Entry(reservation).State = EntityState.Modified;
                    var productIds = cart.Select(item => item.Product_Id).ToList();
                    var products = await context.Products
                        .Where(p => productIds.Contains(p.Product_Id))
                        .ToListAsync();

                    foreach (var item in cart)
                    {
                        var product = products.FirstOrDefault(p => p.Product_Id == item.Product_Id);
                        if (product != null)
                        {
                            product.StockCount += item.Quantity;
                        }
                    }

                    await context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    await transaction.RollbackAsync();
                    await LogAsync(nameof(HandleReservationExpiration), LogUtil.Exception, ex.JSONStringify(), "Concurrency conflict occurred while handling reservation expiration.");
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    await LogAsync(nameof(HandleReservationExpiration), LogUtil.Exception, ex.JSONStringify(), ex.Message);
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private async Task<Reservation?> GetReservationById( string reservationId, EFContext context )
        {
            return await context.Reservations
                .FirstOrDefaultAsync(r => r.Reservation_Id == reservationId);
        }

        private async Task LogAsync( string methodName, string logType, string logMessage, string exception = "" )
        {
            await _logService.Add(new ApiLog
            {
                log_origin = $"ReservationService.{methodName}-{logType}",
                log = logMessage,
                Exception = exception,
                DateTime = DateTime.UtcNow
            });
        }
    }
}