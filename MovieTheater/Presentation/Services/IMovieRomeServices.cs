using WebAPI.Services.DTO.Request;
using WebAPI.Services.DTO.Response;

namespace WebAPI.Services;

public interface IMovieRomeServices
{
    public Task<List<ResquestDTOMovieRoom>> GetAllMovieRoom();
    public Task<ResquestDTOMovieRoom> GetMovieRoomById(int id);
     public Task<bool> addMovieroom(ResquestDTOMovieRoom movieroom);
     public Task<bool> updateMovieroom(ResquestDTOMovieRoom movieroom);
     public Task<bool> deleteMovieroom(int movieroom);

     public Task<(List<ResquestDTOMovieRoom> MovieRooms, int TotalCount,int TotalPages)> GetMovieRoomsPagedAsync(string? search,
         int page, int pageSize);
}