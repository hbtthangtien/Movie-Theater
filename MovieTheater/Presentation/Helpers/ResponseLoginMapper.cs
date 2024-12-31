using AutoMapper;
using WebAPI.Entity;
using WebAPI.Services.DTO.Request;
using WebAPI.Services.DTO.Response;

namespace WebAPI.Helpers
{
    public class ResponseLoginMapper : Profile
    {
        public ResponseLoginMapper()
        {
            //CreateMap<Account, ResponseDTOLogin>().ReverseMap();
        }
    }
}
