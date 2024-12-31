using AutoMapper;
using WebAPI.Entity;
using WebAPI.Services.DTO.Response;

namespace WebAPI.Helpers
{
    public class ScheduleSeatMapper:Profile
    {

        public ScheduleSeatMapper()
        {
            CreateMap<ScheduleSeat, ResponseDTOScheduleSeat>().ReverseMap();
        }
    }
}