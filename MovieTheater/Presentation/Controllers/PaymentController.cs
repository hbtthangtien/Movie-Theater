using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebAPI.Constant;
using WebAPI.Hubs;
using WebAPI.Services;
using WebAPI.Services.DTO.Request;
using WebAPI.Services.DTO.Response;

namespace WebAPI.Controllers
{
    [Route("api/payments")]
    [ApiController]
  
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IHubContext<SeatStatusHub> _hubContext;

        public PaymentController(IPaymentService paymentService, IHubContext<SeatStatusHub> hubContext)
        {
            _paymentService = paymentService;
            _hubContext = hubContext;
        }
        [HttpGet("member/callback")]      
        public async Task<IActionResult> CallBack()
        {
            try
            {
                var payment = await _paymentService.ExecutePayment(Request.Query);
                var redirectUrl = $"http://localhost:3000/ticket?status={payment.Status}&InvoiceId={payment.InvoiceId}";
                string htmlContent = $@"
            <!DOCTYPE html>
            <html lang='en'>
            <head>
                <meta charset='UTF-8'>
                <meta http-equiv='refresh' content='0;url={redirectUrl}' />
                <title>Redirecting...</title>
                <script type='text/javascript'>
                    // JavaScript để tự động chuyển hướng người dùng đến URL chỉ định
                    window.location.replace('{redirectUrl}');
                </script>
            </head>
            <body>
                <p>If you are not redirected automatically, follow this <a href='{redirectUrl}'>link to the payment confirmation page</a>.</p>
            </body>
            </html>";

                return Content(htmlContent, "text/html");
            }
            catch (System.Exception ex)
            {
                return BadRequest(new ResponseDTOApi
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    StatusMessage = ex.Message
                });
            }
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GenerateUrlPayment(RequestDTOPayment request)
        {
            try
            {
                var username = (HttpContext.User?.Identity?.Name) ?? throw new System.Exception("User is not authenticated!!!");
                var payment = await _paymentService.CreatePaymentAsync(request, HttpContext, username);
                return Ok(new
                {
                    vnp_UrlPayment = payment
                });
            }catch(UnauthorizedAccessException e)
            {
                return Unauthorized(new ResponseDTOApi
                {
                    StatusMessage = e.Message,
                    StatusCode = System.Net.HttpStatusCode.Unauthorized
                });
            }
            catch(System.Exception e)
            {
                return BadRequest(new ResponseDTOApi
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    StatusMessage = e.Message
                });
            }


        }

        [HttpPost("employee/generateQr")]
        
        public async Task<IActionResult> GenerateQrPayment(RequestDTOPaymentEmployee request)
        {
            try
            {
                var username = (HttpContext.User?.Identity?.Name) ?? throw new System.Exception("User is not authenticated!!!");
                var payment = await _paymentService.CreatePaymentForEmployeeAsync(request, username);
                return Ok(payment);
            }
            catch(System.Exception ex)
            {
                return BadRequest(new ResponseDTOApi
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    StatusMessage = ex.Message
                });
            }
        }
        [HttpPut("employee/confirm")]
        public async Task<IActionResult> ConfirmPaymentEmployee(RequestDTOPaymentEmployee request)
        {
            try
            {
                var username = (HttpContext.User?.Identity?.Name) ?? throw new System.Exception("User is not authenticated!!!");
                var response =await _paymentService.ComfirmPaymentForEmployee(request, username);
                await _hubContext.Clients.All.SendAsync("confirmSeat", request.ScheduleSeats);
                return Ok(response);
            }
            catch (System.Exception ex) {
                return BadRequest(new ResponseDTOApi
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    StatusMessage = ex.Message
                });
            }
        }

        [HttpPost("member/tickets")]
        public async Task<IActionResult> GenerateTicket(RequestDTOPayment request)
        {
            try
            {
                var username = (HttpContext.User?.Identity?.Name) ?? throw new System.Exception("User is not authenticated!!!");
                var payment =await _paymentService.GenerateTicket(request,username);
                await _hubContext.Clients.All.SendAsync("confirmSeat", request.ScheduleSeats);
                return Ok(payment);
            }catch(System.Exception ex)
            {
                return BadRequest(new ResponseDTOApi
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    StatusMessage = ex.Message
                });
            }
        }
    }
}
