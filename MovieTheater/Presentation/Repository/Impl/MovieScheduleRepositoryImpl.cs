using WebAPI.Entity;

namespace WebAPI.Repository.Impl
{
    public class MovieScheduleRepositoryImpl : GenericRepositoryImpl<MovieSchedule>, IMovieScheduleRepository
    {
        public MovieScheduleRepositoryImpl(MovieTheaterContext context) : base(context)
        {
        }   
        public List<MovieSchedule> GetMovieScheduleByMovieId(string id)
        {
            var result = _context.MovieSchedules.Where(m => m.MovieId == id).ToList();
            return result;
        }
        public IQueryable<MovieSchedule> GetAllMoviesScheduleQueryable()
        {
            return _context.MovieSchedules.AsQueryable();
        }
    }
}
