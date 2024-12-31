using AutoMapper;
using WebAPI.Entity;
using WebAPI.Services.DTO.Request;

namespace WebAPI.Helpers;

public class SeatMapper:Profile
{
    public SeatMapper()
    {
        CreateMap<Seat, RequestDTOSeat>().ReverseMap();
    }
}