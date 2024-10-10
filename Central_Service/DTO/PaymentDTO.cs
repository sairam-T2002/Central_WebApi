using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Central_Service.DTO
{
    public class PaymentDTO
    {
        public string OrderId { get; set; } = string.Empty;

        public string TransactionRef { get; set; } = string.Empty;

        public string PayReceipt { get; set; } = string.Empty;

        public bool Status {  get; set; }

        public string Message { get; set; } = string.Empty;
    }
}
