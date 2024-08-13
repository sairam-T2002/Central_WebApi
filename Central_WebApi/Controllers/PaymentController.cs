using Central_Service.Interface;
using Central_Service.Model;
using Central_Service.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Central_WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        public PaymentController( IPaymentService service)
        {
            _paymentService = service;
        }

        [HttpPost("AcceptPayment")]
        public IPaymentDTO AcceptPayment( [FromBody] PaymentInput input )
        {
            return _paymentService.AcceptPayment(input.input,input.PaymentMethod);
        }
        
    }
}
