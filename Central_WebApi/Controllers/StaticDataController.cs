using Central_Service.Interface;
using Central_Service.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace Central_WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class StaticDataController : ControllerBase
    {
        private readonly IStaticDataService _service;
        private readonly IWebHostEnvironment _env;
        private readonly string _imagePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Images");
        public StaticDataController( IStaticDataService service, IWebHostEnvironment env) {
            _service = service;
            _env = env;
        }

        [HttpGet("AppData")]
        public async Task<StaticDataRet_DTO> GetAppData()
        {
            var webRootPath = _env.WebRootPath;
            var imagePath = Path.Combine(webRootPath, "Images");
            return await _service.ServeStaticData(imagePath).ConfigureAwait(false); ;
        }

        [HttpGet("{filename}")]
        public async Task<IActionResult> GetImage( string filename )
        {
            var filePath = Path.Combine(_imagePath, filename);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "image/jpeg", filename);
        }
    }
}
