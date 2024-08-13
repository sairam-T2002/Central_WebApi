using Central_Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Central_WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AppDataController : ControllerBase
    {
        private readonly IAppDataService _service;
        private readonly IWebHostEnvironment _env;
        #region Constructor
        public AppDataController( IAppDataService service, IWebHostEnvironment env )
        {
            _service = service;
            _env = env;
        }
        #endregion

        [HttpGet("GetHomePageData")]
        public async Task<IActionResult> GetHomePageData()
        {
            var request = HttpContext.Request;
            var url = $"{request.Scheme}://{request.Host}";
            var result = await _service.HomePageData(url).ConfigureAwait(false);
            if(result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
    }
}
