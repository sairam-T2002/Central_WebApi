using Central_Service.Core;
using Central_Service.Interface;
using Microsoft.Extensions.Logging;
using Repository_DAL_.Model;
using Repository_DAL_;
using System.Text;
using Central_Service.DTO;
using ExtensionMethods;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Central_Service.Service
{
    public class ReservationService : ServiceBase, IReservationService
    {
        private readonly IRepository<Reservation> _reservation;
        protected readonly EFContext _context;

        public ReservationService( ILogger<AuthService> logger, IServiceProvider serviceProvider, IRepository<Reservation> reservation, EFContext context ) : base(logger, serviceProvider)
        {
            _reservation = reservation;
            _context = context;
        }

        public async Task<bool> ReserveCart( List<ProductDto> Cart, int userId )
        {
            await UpdateStockcount(Cart);
            string reservationId = $"Reservation_{Guid.NewGuid()}";
            Reservation newReservation = new Reservation
            {
                Reservation_Id = reservationId,
                Cart = Cart.JSONStringify(),
                CreatedTime = DateTime.UtcNow,
                ExpireTime = DateTime.UtcNow.AddMinutes(2),
                IsExpired = false,
                Id = userId
            };
            await _reservation.Add(newReservation);
            _ = Task.Run(() => MonitorReservationStatus(reservationId, CancellationToken.None));
            return true;
        }

        private async void MonitorReservationStatus( string reservationId, CancellationToken cancellationToken )
        {
            using (var scope = CreateScope())
            {
                var context = scope.ServiceProvider.GetService(typeof(EFContext)) as EFContext;
                Reservation reservation;
                while (!cancellationToken.IsCancellationRequested)
                {
                    reservation = await GetReservationById(reservationId, context);
                    if (reservation == null)
                    {
                        // Reservation not found, exit the loop
                        break;
                    }

                    if (reservation.ConfirmedTime != null)
                    {
                        // Reservation is confirmed, exit the loop
                        break;
                    }

                    if (IsReservationExpired(reservation))
                    {
                        await HandleReservationExpiration(reservation, context);
                        break;
                    }

                    // Wait for 20 seconds before the next check
                    await Task.Delay(TimeSpan.FromSeconds(20), cancellationToken);
                }
            }
        }

        private async Task UpdateStockcount( List<ProductDto> Cart )
        {
            if (Cart == null || !Cart.Any())
                return;

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var updates = Cart.Select(item => new { item.Product_Id, item.StockCount }).ToList();
                var query = new StringBuilder();
                var parameters = new List<NpgsqlParameter>();

                for (int i = 0; i < updates.Count; i++)
                {
                    query.Append($"UPDATE public.\"Products\" SET \"StockCount\" = \"StockCount\" - @Quantity{i} WHERE \"Product_Id\" = @ProductId{i};");
                    parameters.Add(new NpgsqlParameter($"@ProductId{i}", updates[i].Product_Id));
                    parameters.Add(new NpgsqlParameter($"@Quantity{i}", updates[i].StockCount));
                }

                await _context.Database.ExecuteSqlRawAsync(query.ToString(), parameters);

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw; // Re-throw the exception after rolling back
            }
        }

        public bool IsReservationExpired( Reservation reservation )
        {
            return DateTime.UtcNow > reservation.ExpireTime;
        }

        private async Task HandleReservationExpiration( Reservation reservation, EFContext context )
        {
            List<ProductDto> Cart = reservation.Cart.JSONParse<List<ProductDto>>();

            var updates = Cart.Select(item => new { item.Product_Id, item.StockCount }).ToList();

            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var query = new StringBuilder();
                var parameters = new List<NpgsqlParameter>();
                parameters.Add(new NpgsqlParameter("@Reservation_Id", reservation.Reservation_Id));
                query.Append("UPDATE public.\"Reservations\" SET \"IsExpired\" = true WHERE \"Reservation_Id\" = @Reservation_Id;");
                for (int i = 0; i < updates.Count; i++)
                {
                    query.Append($"UPDATE public.\"Products\" SET \"StockCount\" = \"StockCount\" + @Quantity{i} WHERE \"Product_Id\" = @ProductId{i};");
                    parameters.Add(new NpgsqlParameter($"@ProductId{i}", updates[i].Product_Id));
                    parameters.Add(new NpgsqlParameter($"@Quantity{i}", updates[i].StockCount));
                }

                await context.Database.ExecuteSqlRawAsync(query.ToString(), parameters);

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw; // Re-throw the exception after rolling back
            }
        }

        private async Task<Reservation> GetReservationById( string reservationId, EFContext context )
        {
            var repo = new Repository<Reservation>(context);
            return (await repo.Find(resrv => resrv.Reservation_Id == reservationId)).FirstOrDefault();
        }
    }
}