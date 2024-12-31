using WebAPI.Entity;

namespace WebAPI.Repository
{
    public interface IMovieScheduleRepository : IGenericRepository<MovieSchedule>
    {
        public List<MovieSchedule> GetMovieScheduleByMovieId(String id);
        public IQueryable<MovieSchedule> GetAllMoviesScheduleQueryable();
    }
}

