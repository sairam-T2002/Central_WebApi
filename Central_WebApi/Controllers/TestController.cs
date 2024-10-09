using Central_Service.Interface;
using Central_WebApi.Core;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Central_WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    #region Private 
    private readonly ITestService _service;
    #endregion
    #region Constructor
    public TestController( ITestService service )
    {
        _service = service;
    }
    #endregion

    #region Public
    [HttpGet("TestMethod")]
    public string TestMethod()
    {
        return _service.TestMethod();
    }
    #endregion
}