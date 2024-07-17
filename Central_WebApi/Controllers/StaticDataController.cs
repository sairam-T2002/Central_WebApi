using Central_Service.Interface;
using Central_Service.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Central_WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class StaticDataController : ControllerBase
    {
        private readonly IStaticDataService _service;
        private readonly IWebHostEnvironment _env;
        public StaticDataController( IStaticDataService service, IWebHostEnvironment env ) {
            _service = service;
            _env = env;
        }

        [HttpGet("AppData")]
        public async Task<StaticDataRet_DTO> GetAppData()
        {
            var webRootPath = _env.WebRootPath;
            var imagePath = Path.Combine(webRootPath, "Images");
            return await _service.ServeStaticData(imagePath);
        }
    }
}
