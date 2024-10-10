using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Central_Service.DTO
{
    public class ReserveCartDTO
    {
        public bool Status { get; set; }
        
        public string Message {  get; set; } = string.Empty;

        public string ReservationId {  get; set; } = string.Empty;
    }

    public class OrderDTOInp
    {
        public string ReservationId { get; set; } = string.Empty;

        public string TransactionRef {  get; set; } = string.Empty;

        public int UserId { get; set; }

        public string PaymentMethod { get; set; } = string.Empty;

        public double AmountPaid { get; set; }

        public string Status { get; set; } = string.Empty;
    }

    public class OrderDTOOut
    {
        public string OrderId { get; set; } = string.Empty;

        public string TransactionRef { get; set; } = string.Empty;

        public bool Status { get; set;}

        public string Message { get; set; } = string.Empty;
    }
}
