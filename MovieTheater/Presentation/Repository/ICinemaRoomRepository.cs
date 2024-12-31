using WebAPI.Entity;

namespace WebAPI.Repository
{
    public interface ICinemaRoomRepository : IGenericRepository<CinemaRoom>
    {
        public Task<CinemaRoom> AddCinemaRoom(CinemaRoom cinemaRoom);
        public Task<bool> updateinemaRoom(CinemaRoom cinemaRoom);
        public Task<bool> deleteMovieroom(int movieroom);
        IQueryable<CinemaRoom> GetAllQueryable();
        IQueryable<CinemaRoom> GetMovieRoomsByName(string name);
        public Task<CinemaRoom> GetMovieRoomByID(int id);
        public Task<bool> autoAddSeat(int cinemaID, int? seatQuantity);
    }
}
