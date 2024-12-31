using AutoMapper;
using WebAPI.Repository;

namespace WebAPI.Services
{
    public class GenericServices
    {
        protected readonly IMapper _mapper;

        protected readonly IUnitOfWork _context;
        
        public GenericServices(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _context = unitOfWork;
        }

    }
}
