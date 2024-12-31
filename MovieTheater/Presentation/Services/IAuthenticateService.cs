using WebAPI.Entity;
using WebAPI.Services.DTO;
using WebAPI.Services.DTO.Request;
using WebAPI.Services.DTO.Response;

namespace WebAPI.Services
{
    public interface IAuthenticateService
    {
        public Task<ResponseDTOToken> createToken(ApplicationUser user);
        public Task<ResponseDTOToken> refreshToken(string refresh_token);
        public Task<ResponseDTOToken> Login(RequestDTOLogin account);
        public Task RemoveRefreshToken(string usrename);
    }
}
