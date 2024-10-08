using Central_Service.Core;
using Central_Service.Interface;
using Microsoft.Extensions.Logging;
using Repository_DAL_.Model;
using Repository_DAL_;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Central_Service.DTO;
using ExtensionMethods;

namespace Central_Service.Service
{
    public class ReservationService : ServiceBase, IReservationService
    {
        private readonly IRepository<Reservation> _reservation;
        public ReservationService( ILogger<AuthService> logger, IServiceProvider serviceProvider, IRepository<Reservation> reservation ) : base(logger, serviceProvider)
        {
            _reservation = reservation;
        }

        public async Task<bool> ReserveCart( List<ProductDto> Cart, string userName )
        {
            string reservationId = Guid.NewGuid().ToString();
            Reservation newReservation = new Reservation {
                Reservation_Id = reservationId,
                Cart = Cart.JSONStringify(),
                CreatedTime = DateTime.UtcNow,
                ExpireTime = DateTime.UtcNow.AddMinutes(10),
            };
            await _reservation.Add(newReservation);
            return true;
        }

        public async Task UpdateStockcount( List<ProductDto> Cart )
        {

        }
    }
}
