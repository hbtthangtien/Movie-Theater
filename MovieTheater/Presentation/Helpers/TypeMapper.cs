using AutoMapper;
using WebAPI.Services.DTO;
using Type = WebAPI.Entity.Type;


namespace WebAPI.Helpers
{
    public class TypeMapper : Profile
    {
        public TypeMapper()
        {
            CreateMap<Type,TypeDTO>().ReverseMap();
        }
    }
}