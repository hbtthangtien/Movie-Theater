using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Constant;
using WebAPI.Entity;
using WebAPI.Repository;
using WebAPI.Services;
using WebAPI.Services.DTO.Request;

namespace WebAPI.Controllers
{
    [Route("api/roles")]
    [ApiController]
    [Authorize(Roles =UserRole.MEMBER)]
    public class RoleController : ControllerBase
    {
        private readonly IUnitOfWork _movieContext;
        private readonly IQRCodeServicee _qrcode;

        public RoleController(IUnitOfWork movieTheater, IQRCodeServicee qRCodeServicee)
        {
            _movieContext = movieTheater;
            _qrcode = qRCodeServicee;
        }
        
        [HttpDelete]
        
        public async Task<IActionResult> Get(RequestDTOQRCode request)
        {
            return Ok(await _movieContext.Movies.GetAllAsync());
        }



    }
}
