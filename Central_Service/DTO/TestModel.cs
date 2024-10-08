
using Central_Service.Payment;

namespace Central_Service.DTO
{
    public class TestModel
    {
        public string TestName { get; set; }
        public string TestValue { get; set; }
        public int TestId { get; set; }
    }

    public class PaymentInput
    {
        public IPaymentDTO input { get; set; }
        public string PaymentMethod { get; set; }
    }
}
