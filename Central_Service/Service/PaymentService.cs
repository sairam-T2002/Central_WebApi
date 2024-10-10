using Central_Service.Core;
using Central_Service.Interface;
using Central_Service.Payment;
using Microsoft.Extensions.Logging;
using Repository_DAL_.Model;
using Repository_DAL_;
using Central_Service.DTO;

namespace Central_Service.Service;

public class PaymentService : ServiceBase, IPaymentService
{
    #region Private Declarations

    private readonly IRepository<Reservation> _reservationRepository;

    #endregion
    public PaymentService( ILogger<PaymentService> logger, IServiceProvider serviceProvider, IRepository<Reservation> reservation ) : base(logger, serviceProvider)
    {
        _reservationRepository = reservation;
    }

    #region Services

    public async Task<PaymentDTO> AcceptPayment( IPaymentDTO input, string PaymentMethod )
    {
        IPaymentDTO? paymentDTO = null;
        if (PaymentMethod == "CARD")
        {
            var temp = paymentDTO as CardPayment;
        }
        else
        {
            var temp = paymentDTO as UPIPayment;
        }

        return new PaymentDTO();
    }

    #endregion
}