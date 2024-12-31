using AutoMapper;
using WebAPI.Entity;
using WebAPI.Services.DTO;
using WebAPI.Services.DTO.Response;

namespace WebAPI.Helpers
{
    public class MovieMapper : Profile
    {
        public MovieMapper()
        {
            
            CreateMap<Movie, MovieDTO>() .ForMember(dest => dest.Types, opt => opt.MapFrom(src => src.Types))
                .ReverseMap();
            CreateMap<Movie, ResponseDTOMovie>()
                .ReverseMap();
        }
    }
}
