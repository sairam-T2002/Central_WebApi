using Central_Service.Interface;
using Central_Service.Model;
using Central_WebApi.Core;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Central_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors(AppConstants.CROS_POLICY_NAME)]
    public class TestController : ControllerBase
    {
        #region Private 
        private readonly ITestService _service;
        #endregion
        #region Constructor
        public TestController( ITestService service )
        {
            this._service = service;
        }
        #endregion

        #region Public
        [HttpGet("TestMethod")]
        public async Task<string> TestMethod()
        {
            return await this._service.TestMethod().ConfigureAwait(false);
        }
        #endregion
    }
}
