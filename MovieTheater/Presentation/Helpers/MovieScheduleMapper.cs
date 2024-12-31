using AutoMapper;
using WebAPI.Entity;
using WebAPI.Services.DTO;

namespace WebAPI.Helpers
{
    public class MovieScheduleMapper : Profile
    {
        public MovieScheduleMapper()
        {
            CreateMap<MovieSchedule, MovieScheduleDTO>().ReverseMap();
        }
    }
}