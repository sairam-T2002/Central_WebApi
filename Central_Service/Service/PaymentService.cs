using Central_Service.Core;
using Central_Service.Interface;
using Central_Service.Payment;
using Microsoft.Extensions.Logging;

namespace Central_Service.Service;

    public class PaymentService : ServiceBase, IPaymentService
    {
        public PaymentService(ILogger<PaymentService> logger, IServiceProvider serviceProvider ) : base(logger, serviceProvider)
        {

        }
        public IPaymentDTO AcceptPayment( IPaymentDTO input, string PaymentMethod )
        {
            IPaymentDTO? paymentDTO = null;

            if(PaymentMethod == "CARD")
            {
                paymentDTO = new CardPayment();
            }
            else
            {
                paymentDTO = new UPIPayment();
            }

            return paymentDTO;
        }
    }