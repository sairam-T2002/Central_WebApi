using Central_Service.Core;
using Central_Service.Interface;

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
