using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Central_Service.Core;
using Central_Service.Interface;
using Central_Service.Model;

namespace Central_Service.Service
{
    public class TestService : CoreClass, ITestService
    {
        #region Constructor
        public TestService( IServiceProvider serviceProvider ) : base(serviceProvider) { }
        #endregion

        #region Public Method
        public async Task<string> TestMethod()
        {
            return "Hello World - from TestMethod";
        }

        #endregion
    }
}
