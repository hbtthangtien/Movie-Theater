using AutoMapper;
using WebAPI.Entity;
using WebAPI.Services.DTO;
using WebAPI.Services.DTO.Request;
using WebAPI.Services.DTO.Response;

namespace WebAPI.Helpers
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper() { 
        
            CreateMap<RequestDTORegister,ApplicationUser>().ReverseMap();
            CreateMap<ApplicationUser, ResponseDTOLogin>().ReverseMap();
            CreateMap<RequestDTOPayment,RequestDTOPaymentEmployee>().ReverseMap();  
            CreateMap<ApplicationUser,ResponseDTOMemberCheck>().ReverseMap();
            CreateMap<ResponseDTOSeats, ScheduleSeat>().ReverseMap()
                    .ForMember(dest => dest.SeatType, otp => otp.MapFrom(src => new seatTypeDTO
                    {
                        Name = src.TypeSeat.Name,
                        price = (double)src.TypeSeat.price,
                        SeatTypeId = src.seatType_id
                    }));
        }
    }
}
