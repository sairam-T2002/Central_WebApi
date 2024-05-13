using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Central_Service.Model
{
    public class SessionInfo
    {
        public string MachineID { get; set; }
        public string BrowserInfo { get; set; }
        public string UserID { get; set; }
    }
    public class OperationStatus
    {
        public bool IsSuccessful { get; set; }
        public string Message { get; set; }
        public string ErrorCode { get; set; }
    }
}
