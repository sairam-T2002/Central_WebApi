using Central_Service.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Central_Service.Interface
{
    public interface IPaymentService
    {
        Task<IPaymentDTO> AcceptPayment( IPaymentDTO input,string PaymentMethod);
    }
}
