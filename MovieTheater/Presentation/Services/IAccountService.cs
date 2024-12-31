using WebAPI.Entity;
using WebAPI.Services.DTO.Request;
using WebAPI.Services.DTO.Response;

namespace WebAPI.Services
{
    public interface IAccountService
    {
        public Task CreateAccount(RequestDTORegister accountDTO);

        public Task<ResponseDTOApi> CreateAccountEmployee(RequestDTORegister accountDTO);
        public Task<ResponseDTOApi> EditMember(string userName, RequestDTOMyAccount memberDTO, IFormFile? profileImage);

        public Task<ResponseDTOApi> ChangePassword(string userName, RequestDTOChangePassword passwordDTO);

        public Task<ResponseDTOMyAccount> GetAccountMember(string userName);

        public Task<ResponseDTOMemberCheck> CheckExistMember(string member);
        
        public Task<double> GetScoreByIdMember(string member);
    }
}
