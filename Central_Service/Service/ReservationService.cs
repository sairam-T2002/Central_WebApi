using Central_Service.Core;
using Central_Service.Interface;
using Microsoft.Extensions.Logging;
using Repository_DAL_.Model;
using Repository_DAL_;
using Central_Service.DTO;
using ExtensionMethods;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Central_Service.Service;

public class ReservationService : ServiceBase, IReservationService
{
    #region Private Declarations

    private readonly IRepository<Reservation> _reservationRepository;
    private readonly IRepository<Orders> _ordersRepository;
    private readonly IRepository<ApiLog> _logService;
    private readonly EFContext _context;
    private readonly IObjectFactory _factory;

    #endregion

    #region Semaphore Declaration

    private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    #endregion

    public ReservationService( ILogger<ReservationService> logger, IServiceProvider serviceProvider, IRepository<Reservation> reservationRepository, IRepository<Orders> ordersRepository, IRepository<ApiLog> logService, EFContext context, IObjectFactory factory ) : base(logger, serviceProvider)
    {
        _reservationRepository = reservationRepository;
        _context = context;
        _logService = logService;
        _factory = factory;
        _ordersRepository = ordersRepository;
    }

    #region Private Methods

    private async Task<bool> UpdateStockCount( List<ProductDto> cart )
    {
        await _semaphore.WaitAsync();
        try
        {
            if (cart == null || !cart.Any())
                return true;
            var productIds = cart.Select(item => item.Product_Id).ToList();
            var products = await _context.products.Where(p => productIds.Contains(p.product_id)).ToListAsync();
            foreach (var item in cart)
            {
                var product = products.FirstOrDefault(p => p.product_id == item.Product_Id);
                if (product == null || product.stockcount < item.Quantity)
                {
                    return false;
                }
                product.stockcount -= item.Quantity;
            }
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                await LogAsync(nameof(UpdateStockCount), LogUtil.Exception, ex.JSONStringify(), ErrorMessages.ReservationException);
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
                if (reservation == null || reservation.confirmedtime != null)
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
        return DateTime.UtcNow > reservation.expiretime;
    }

    private async Task HandleReservationExpiration( Reservation reservation, EFContext context )
    {
        await _semaphore.WaitAsync();
        try
        {
            List<ProductDto> cart = reservation.cart.JSONParse<List<ProductDto>>();
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                reservation.isexpired = true;
                context.Entry(reservation).State = EntityState.Modified;
                var productIds = cart.Select(item => item.Product_Id).ToList();
                var products = await context.products
                                            .Where(p => productIds.Contains(p.product_id))
                                            .ToListAsync();
                foreach (var item in cart)
                {
                    var product = products.FirstOrDefault(p => p.product_id == item.Product_Id);
                    if (product != null)
                    {
                        product.stockcount += item.Quantity;
                    }
                }
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                await Task.WhenAll(
                transaction.RollbackAsync(),
                LogAsync(nameof(HandleReservationExpiration), LogUtil.Exception, ex.JSONStringify(), ErrorMessages.ReservationException)
                );
            }
            catch (Exception ex)
            {
                await Task.WhenAll(
                transaction.RollbackAsync(),
                LogAsync(nameof(HandleReservationExpiration), LogUtil.Exception, ex.JSONStringify(), ex.Message)
                );
            }
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private async Task<Reservation?> GetReservationById( string reservationId, EFContext context )
    {
        return await context.reservations
            .FirstOrDefaultAsync(r => r.reservation_id == reservationId);
    }

    private async Task LogAsync( string methodName, string logType, string logMessage, string exception = "" )
    {
        await _logService.Add(new ApiLog
        {
            log_origin = $"ReservationService.{methodName}-{logType}",
            log = logMessage,
            exception = exception,
            datetime = DateTime.UtcNow
        });
    }

    #endregion

    #region Services

    public async Task<ReserveCartDTO> ReserveCart( List<ProductDto> cart, int userId )
    {
        var output = new ReserveCartDTO {
            Status = false,
            Message = ReservationStatus.FailedReservation,
        };
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            if (!await UpdateStockCount(cart))
            {
                await transaction.RollbackAsync();
                return output;
            }
            var newReservation = _factory.BuildReservationDomain(cart,false, userId);
            await _reservationRepository.Add(newReservation);
            await transaction.CommitAsync();
            _ = Task.Run(() => MonitorReservationStatus(newReservation.reservation_id, CancellationToken.None));
            output.Status = true;
            output.Message = ReservationStatus.SuccessfulReservation;
            output.ReservationId = newReservation.reservation_id;
            return output;
        }
        catch (Exception ex)
        {
            await Task.WhenAll(
            transaction.RollbackAsync(), 
            LogAsync(nameof(ReserveCart), LogUtil.Exception, ex.JSONStringify(), ex.Message)
            );
            return output;
        }
    }

    public async Task<OrderDTOOut> ConfirmReservation( OrderDTOInp input)
    {
        var output = new OrderDTOOut {
            Status = false,
            Message = ErrorMessages.TimeOut
        };
        try
        {
            var order = _factory.BuildOrderDomain(input);
            await _ordersRepository.Add(order);
            output = new OrderDTOOut {
                OrderId = order.order_id,
                TransactionRef = order.transactionref,
                Status = true,
                Message = ReservationStatus.SuccessfulOrder,
            };
        }
        catch(Exception ex)
        {
            output.Message = ex.Message;
            output.Status = false;
        }
        return output;
    }

    #endregion
}