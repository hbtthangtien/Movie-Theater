
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebAPI.Services;
using WebAPI.Services.DTO.Request;
using WebAPI.Services.DTO.Response;

namespace WebAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public UsersController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> CreateAccount(RequestDTORegister request)
        {
            try
            {
                await _accountService.CreateAccount(request);
                return Ok();
            }
            catch(System.Exception ex)
            {
                return BadRequest(new ResponseDTOApi
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    StatusMessage = ex.Message
                });
            }
        }

        [HttpGet("{member}")]
        public async Task<IActionResult> CheckExistUser(string member)
        {
            try
            {
                var m = await _accountService.CheckExistMember(member);
                return Ok(m);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new ResponseDTOApi
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    StatusMessage = ex.Message
                });
            }
        }

        [HttpGet("scores/{AccountId}")]
        public async Task<IActionResult> GetScoreById(string AccountId)
        {
            try
            {
                double score = await _accountService.GetScoreByIdMember(AccountId);
                return Ok(new
                {
                    score = score
                });
            }
            catch(System.Exception ex)
            {
                return BadRequest(new ResponseDTOApi
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    StatusMessage = ex.Message
                });
            }
        }

    }
}
