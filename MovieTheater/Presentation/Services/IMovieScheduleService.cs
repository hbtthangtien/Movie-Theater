using WebAPI.Services.DTO;

namespace WebAPI.Services
{
    public interface IMovieScheduleService
    {
        public List<MovieScheduleDTO> GetMovieScheduleByMovieID(String id);

    }
}