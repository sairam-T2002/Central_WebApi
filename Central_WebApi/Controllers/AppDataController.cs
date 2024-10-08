using Central_Service.Interface;
using Central_Service.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

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

        [HttpGet("GetSearchResults/{category}")]
        public async Task<IActionResult> GetSearchResults( string category, string? searchquery = null )
        {
            var request = HttpContext.Request;
            var url = $"{request.Scheme}://{request.Host}";

            var result = await _service.SeachResult(url, category, searchquery ?? "").ConfigureAwait(false);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet("GetAppConfig")]
        public async Task<IActionResult> GetAppConfig()
        {
            var result = await _service.AppConfig().ConfigureAwait(false);
            return Ok(new { googleMapsApiKey = result });
        }

        [HttpPost("SaveCart")]
        public async Task<IActionResult> SaveCart(List<ProductDto> cart, string username)
        {
            var result = await _service.SaveCart(cart, username).ConfigureAwait(false);
            if(result == 1)
            {
                return Ok(new { Status=true,Message="Cart saved"});
            }
            else if( result == -1)
            {
                return NotFound(new {Status=false,Message="Invalid User"});
            }
            else
            {
                return NotFound(new { Status = false, Message = "Server Exception" });
            }
        }
    }
}
