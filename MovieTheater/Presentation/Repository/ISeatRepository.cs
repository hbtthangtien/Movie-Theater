using WebAPI.Entity;

namespace WebAPI.Repository
{
    public interface ISeatRepository : IGenericRepository<Seat>
    {
        public Task<bool> AddSeat(List<Seat> seat);
        public Task<bool> UpdateSeat(List<Seat> seat);
        public Task<bool> DeleteSeat(List<int> seatIds);
        
        public Task<List<Seat>> GetAllSeatByCinemaID(int cinemaId);
    }
}
