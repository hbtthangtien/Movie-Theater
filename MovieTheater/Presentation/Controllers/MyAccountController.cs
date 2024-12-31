using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebAPI.Services;
using WebAPI.Services.DTO.Request;

namespace WebAPI.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class MyAccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private string? username => HttpContext.User?.Identity?.Name;
        public MyAccountController(IAccountService accountService) {
            _accountService = accountService;
        }

        [HttpGet("member/accounts")]
        public async Task<ActionResult> getInformationOfAccount() {
            try
            {
                if (string.IsNullOrEmpty(username))
                {
                    return BadRequest(new { message = "Your login time has expired, please log in again to continue." });
                }
                var member = await _accountService.GetAccountMember(username);
                if (member == null)
                {
                    return NotFound("No infomation of account !!!");
                }else { return Ok(member); }
            }catch(System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("member/accounts")]
        

        [HttpPut("member/accounts/password")]
        public async Task<IActionResult> ChangePassword( [FromBody] RequestDTOChangePassword passwordDTO)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest(new { message = "Your login time has expired, please log in again to continue." });
            }

            var response = await _accountService.ChangePassword(username, passwordDTO);

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return BadRequest(new { message = response.StatusMessage,  errors = response.errors });
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound(new { message = response.StatusMessage, errors = response.errors });
            }

            return Ok(new { message = response.StatusMessage, errors = response.errors });
        }

    }
}
