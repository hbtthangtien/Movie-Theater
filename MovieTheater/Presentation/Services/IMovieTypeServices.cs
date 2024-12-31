using WebAPI.Services.DTO;

namespace WebAPI.Services
{
    public interface IMovieTypeServices
    {
        public List<TypeDTO> GetAllMovieTypes();

    }
}