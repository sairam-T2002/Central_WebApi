using Central_Service.Interface;
using Central_Service.DTO;
using Central_Service.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Central_WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PaymentController : ControllerBase
{
    #region Private Declarations

    private readonly IPaymentService _paymentService;

    #endregion

    #region Constructor

    public PaymentController( IPaymentService service )
    {
        _paymentService = service;
    }

    #endregion

    #region Actions

    [HttpPost("AcceptPayment")]
    public IPaymentDTO AcceptPayment( [FromBody] PaymentInput input )
    {
        return _paymentService.AcceptPayment(input.input, input.PaymentMethod);
    }

    #endregion
}