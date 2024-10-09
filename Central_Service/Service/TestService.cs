using Central_Service.Core;
using Central_Service.Interface;
using Microsoft.Extensions.Logging;

namespace Central_Service.Service;

public class TestService : ServiceBase, ITestService
{
    #region Constructor
    public TestService( IServiceProvider serviceProvider, ILogger<TestService> logger ) : base(logger, serviceProvider) { }
    #endregion

    #region Public Method
    public string TestMethod()
    {
        return "Hello World - from TestMethod";
    }

    #endregion
}