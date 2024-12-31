using AutoMapper;
using WebAPI.Entity;
using WebAPI.Services.DTO.Request;

namespace WebAPI.Helpers
{
    public class AccountMapper : Profile
    {
        public AccountMapper()
        {
            //CreateMap<Account,RequestDTORegister>().ReverseMap();

        }
    }
}
