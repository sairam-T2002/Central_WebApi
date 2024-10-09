using Central_Service.Payment;

namespace Central_Service.Interface
{
    public interface IPaymentService
    {
        IPaymentDTO AcceptPayment( IPaymentDTO input,string PaymentMethod);
    }
}
