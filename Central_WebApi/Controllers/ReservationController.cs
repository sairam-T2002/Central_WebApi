﻿using Central_Service.DTO;
using Central_Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Central_WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _service;

        public ReservationController( IReservationService service)
        {
            _service = service;
        }

        [HttpPost("ReserveCart")]
        public async Task<IActionResult> ReserveCart([FromBody]List<ProductDto> cart, int userId)
        {
            bool reservationResult = await _service.ReserveCart(cart,userId);
            if (reservationResult) return Ok(new { Status = true, Message = "Reservation Successful" });
            else return Ok(new { Status = false, Message = "Reservation Failed" });
        }
    }
}
