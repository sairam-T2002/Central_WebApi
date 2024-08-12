using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Central_Service.Payment
{
    public class CardPayment : IPaymentDTO
    {
        public string CardNumber { get; set; }
        public string CardType { get; set; }
        public string CardHolderName { get; set; }
        public string ValidThrough { get; set; }
        public int CVV {  get; set; }
    }
}
