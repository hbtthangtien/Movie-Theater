using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Entity;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/member")]
    [ApiController]
    [Authorize]
    public class HistoryController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public HistoryController(IInvoiceService invoiceService) {
            _invoiceService = invoiceService;
        }

        [HttpGet("tickets")]
        public async Task<IActionResult> ViewBookedTicketsHistory(string movieName ="", int page = 1)
        {

            var username = (HttpContext.User?.Identity?.Name);
            try {
                var history = await _invoiceService.GetInvoiceForBookedTicket(username, movieName, page);
                if (history == null || !history.Any())
                {
                    return NotFound("No invoices found for the specified user and movie.");
                }else
                {
                    return Ok(history);
                }
            }
            catch(System.Exception e) {
                return BadRequest("You need to login to find invoice.");
            }
        }
        [HttpGet("tickets/{id}")]
        public async Task<IActionResult> ViewBookedTicketsDetail(string id)
        {

            try
            {
                var invoice = await _invoiceService.ViewDetailInvoice(id);
                if (invoice == null)
                {
                    return NotFound("No found invoices !!!");
                }
                else
                {
                    return Ok(invoice);
                }
            }
            catch (System.Exception e)
            {
                return BadRequest("You need to login to find invoice.");
            }
        }

        [HttpGet("scores")]
        public async Task<IActionResult> ViewHistoryScore(string? fromDate, string? toDate, string historyType, int page = 1)
        {

            var username = (HttpContext.User?.Identity?.Name);
            try
            {
                DateTime? from = string.IsNullOrEmpty(fromDate) ? (DateTime?)null : DateTime.Parse(fromDate);
                DateTime? to = string.IsNullOrEmpty(toDate) ? (DateTime?)null : DateTime.Parse(toDate);
                var history = await _invoiceService.GetInvoiceForHistoryScore(from, to, username, historyType, page);
                if (history == null || !history.Any())
                {
                    return NotFound("No invoices found for the specified user and movie.");
                }
                else
                {
                    return Ok(history);
                }
            }
            catch (System.Exception e)
            {
                return BadRequest(new { message = "An error occurred while fetching the score history.", error = e.Message });
            }
        }

    }
}
