using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Central_Service.DTO
{
    public static class LogUtil
    {
        public static string Request { get; } = "Request";

        public static string Response { get; } = "Response";

        public static string Exception { get; } = "Exception";
    }
}
