using AutoMapper;
using WebAPI.Repository;
using WebAPI.Repository.Impl;
using WebAPI.Services.DTO;
using WebAPI.Entity;
using Type = WebAPI.Entity.Type;

namespace WebAPI.Services.Impl
{
    public class MovieTypeImpl : GenericServices, IMovieTypeServices
    {
        public MovieTypeImpl(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
        {
        }


        public List<TypeDTO> GetAllMovieTypes()
        {
            var type = _context.TypeRepository.GetAllTypes();
            
            return _mapper.Map<List<TypeDTO>>(type);
        }
    }
}