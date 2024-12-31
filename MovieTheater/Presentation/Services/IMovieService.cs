using WebAPI.Services.DTO;
using WebAPI.Services.DTO.Response;

namespace WebAPI.Services
{
    public interface IMovieService
    {
        public Task<MovieDTO> GetMovieById(String id);
        public Task<List<MovieDTO>> GetAllMovies();
        public Task<List<MovieDTO>> GetMovieByName(string name);
        public Task<List<MovieDTO>> GetMovieInComing();
        public Task<List<MovieDTO>> GetMovieUpComing();
        
        public Task<bool> UpdateMovie(MovieDTO movieDTO,IFormFile? largeImage, IFormFile? smallImage);
        
        public Task<bool> DeleteMovieAsync(string movieId);
        public Task<(bool Success, string? Errors)> AddMovieAsync(ResponseDTOMovie movie, IFormFile? largeImage, IFormFile? smallImage);

        public Task<(List<MovieDTO> Movies, int TotalCount,int TotalPages)>
            GetMoviesPagedAsync(string? search, int page, int pageSize,int status);
        public Task<(List<MovieDTO> Movies, int TotalCount,int TotalPages)>
            GetMoviesincomingAsync(string? search, int page, int pageSize);
    }
}