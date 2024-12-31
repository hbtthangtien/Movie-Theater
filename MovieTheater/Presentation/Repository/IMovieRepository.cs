using WebAPI.Entity;
using Type = System.Type;

namespace WebAPI.Repository
{
    public interface IMovieRepository : IGenericRepository<Movie>
    {
        
        public Task<List<Movie>> GetAllMovies();
        public List<Movie> GetMovieByName(string name);
        public Task<List<Movie>> GetMovieInComing();
        public Task<List<Movie>> GetMovieUpcoming();
        public List<String>? getMovieTypeNameByMovieID(String movieID);
        public Task<Movie> GetMovieByIDAsync(string ID);
        public Task<bool> DeleteMovieAsync(string movieId);
        
        public Task<bool> UpdateMovieAsync(Movie movie);
        
        public Task<string> AddMovieAsync(Movie movie);
        public Task<bool> addMovieType(String movieID,int typeID);
        public Task<bool> updateMovieroom(string movieID, int roomId);
        IQueryable<Movie> GetAllMoviesQueryable();
        IQueryable<Movie> GetMoviesByNameQueryable(string name);
        IQueryable<Movie> GetIncomingMoviesQueryable();
        IQueryable<Movie> GetMoviesByNameDateQueryable(string name);
        public Task<Movie> getMovieByMovieName(string name);
    }
}