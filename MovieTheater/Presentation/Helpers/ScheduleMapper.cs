using AutoMapper;
using WebAPI.Entity;
using WebAPI.Services.DTO.Request;
using WebAPI.Services.DTO.Response;

namespace WebAPI.Helpers
{
    public class ScheduleMapper:Profile
    {
        public ScheduleMapper()
        {
            CreateMap<Schedule, ScheduleDTO>().ReverseMap();
            CreateMap<Schedule, RequestDTOSchedule>().ReverseMap();
            CreateMap<Schedule, ResponseDTOSchedule>().ReverseMap();
        }
    }
}