using Central_Service.DTO;
using Central_Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Central_WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ReservationController : ControllerBase
{
    #region Private Declarations

    private readonly IReservationService _service;

    #endregion

    #region Constructor

    public ReservationController( IReservationService service )
    {
        _service = service;
    }

    #endregion

    #region Actions

    [HttpPost("ReserveCart")]
    public async Task<IActionResult> ReserveCart( [FromBody] List<ProductDto> cart, int userId )
    {
        var reservationResult = await _service.ReserveCart(cart, userId);
        return Ok(reservationResult);
    }

    #endregion
}