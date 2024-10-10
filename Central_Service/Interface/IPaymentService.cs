using Central_Service.DTO;
using Central_Service.Payment;

namespace Central_Service.Interface;

public interface IPaymentService
{
    Task<PaymentDTO> AcceptPayment( IPaymentDTO input, string PaymentMethod );
}