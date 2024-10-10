using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Central_Service.Core
{
    public static class OrderStatus
    {
        public static string OrderSuccessful { get; } = "CONFIRMED";
        public static string OrderFailed { get; } = "FAILED";
        public static string PaymentFailed { get; } = "PAYMENTFAILED";
        public static string InternalException { get; } = "EXCEPTION";

        
    }

    public static class ReservationStatus
    {
        public static string TimeOut { get; } = "Order timeout";
        public static string SuccessfulReservation { get; } = "Reservation Successful";
        public static string FailedReservation { get; } = "Reservation Failed";
        public static string SuccessfulOrder { get; } = "Order Placed Successful";
    }

    public static class ErrorMessages
    {
        public static string TimeOut { get; } = "Order timeout";
        public static string ReservationException { get; } = "Concurrency conflict occurred while handling reservation expiration";
    }
}
