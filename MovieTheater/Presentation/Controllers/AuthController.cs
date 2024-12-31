using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Exception;
using WebAPI.Services;
using WebAPI.Services.DTO.Request;
using WebAPI.Services.DTO.Response;

namespace WebAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticateService _authenticate;

        public AuthController(IAuthenticateService authenticate)
        {
            _authenticate = authenticate;    
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(RequestDTOLogin request)
        {
            try
            {
                var token = await _authenticate.Login(request);             
                saveTokenToCookie(token, HttpContext);
                return Ok(token);
            }catch(NotFoundException e)
            {
                return NotFound(new ResponseDTOApi
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    StatusMessage = e.Message
                });
            }catch(System.Exception e)
            {
                return BadRequest(new ResponseDTOApi
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    StatusMessage = e.Message
                });
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                string refresh_token = getRefreshTokenFromCookie();
                var token = await _authenticate.refreshToken(refresh_token);
                saveTokenToCookie(token, HttpContext);
                return Ok(token);
            }catch(NotFoundException e)
            {
                return NotFound(new ResponseDTOApi
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    StatusMessage = e.Message
                });
            }
            catch (System.Exception e)
            {
                return BadRequest(new ResponseDTOApi
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    StatusMessage = e.Message
                });
            }
        }
        [Authorize]
        [HttpDelete("log-out")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var username = (HttpContext.User?.Identity?.Name) ?? throw new System.Exception("User is not authenticated!!!");
                await _authenticate.RemoveRefreshToken(username);
                RemoveTokenInsideCookie(HttpContext);
                await HttpContext.SignOutAsync();
                return Ok("Logout successfull");
            }catch(System.Exception e)
            {
                return BadRequest(new ResponseDTOApi
                {
                    StatusCode =System.Net.HttpStatusCode.BadRequest,
                    StatusMessage = $"Logout failed because {e.Message}"
                });
            }

        }

        private void RemoveTokenInsideCookie(HttpContext context)
        {
            context.Response.Cookies.Append("accessToken", "", new CookieOptions
            {
                HttpOnly = false,
                Expires = DateTimeOffset.UtcNow.AddDays(-1)
            });
            context.Response.Cookies.Append("refreshToken", "", new CookieOptions
            {
                HttpOnly = false,
                Expires = DateTimeOffset.UtcNow.AddDays(-1)
            });
        }

        private void saveTokenToCookie(ResponseDTOToken token, HttpContext context)
        {
            
            context.Response.Cookies.Append("accessToken", token.AccessToken,
            new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddMinutes(30),
                HttpOnly = true,
                IsEssential = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });
            const string refreshTokenPath = "/api/auth/refresh-token";
            context.Response.Cookies.Append("refreshToken", token.RefreshToken,
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(7),
                    HttpOnly = true,
                    IsEssential = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Path = refreshTokenPath,
                });
        }
        private string getRefreshTokenFromCookie()
        {
            
            if(HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refresh_token))
            {
                if (string.IsNullOrEmpty(refresh_token))
                {
                    throw new System.Exception("Refresh token is null");
                }
                return refresh_token;
            }else
            {
                throw new NotFoundException("No refresh token");
            }
        }
    }
    
}
