using AutoMapper;
using WebAPI.Repository;
using WebAPI.Repository.Impl;
using WebAPI.Services.DTO;

namespace WebAPI.Services.Impl
{
    public class MovieScheduleServiceImpl :GenericServices, IMovieScheduleService
    {
        public MovieScheduleServiceImpl(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
        {
        }

        public List<MovieScheduleDTO> GetMovieScheduleByMovieID(string id)
        {
            var movieSchedule = _context.MovieSchedules.GetMovieScheduleByMovieId(id);
            return _mapper.Map<List<MovieScheduleDTO>>(movieSchedule);

        }
    }
}