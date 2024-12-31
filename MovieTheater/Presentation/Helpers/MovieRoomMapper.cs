using AutoMapper;
using WebAPI.Entity;
using WebAPI.Services.DTO.Request;

namespace WebAPI.Helpers;

public class MovieRoomMapper:Profile
{
    public MovieRoomMapper()
    {
        CreateMap<ResquestDTOMovieRoom, CinemaRoom>().ReverseMap();
    }
}