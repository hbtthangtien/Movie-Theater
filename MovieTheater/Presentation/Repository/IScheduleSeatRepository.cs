using WebAPI.Entity;
using WebAPI.Services.DTO.Request;
using WebAPI.Services.DTO.Response;

namespace WebAPI.Repository
{
    public interface IScheduleSeatRepository : IGenericRepository<ScheduleSeat>
    {
        public List<ScheduleSeat> GetScheduleSeatsBymovieidAndschid(String movieId, int schId);
        public string? GetMovieCinemaNameBySeatId(int? seatId);
        public double GetPriceBySeatTypeId(int? seatTypeId);
        public string? GetNameBySeatTypeId(int? seatTypeId);
        public string? GetMovieNameVNByMovieId(string? Id);
        public string? GetMovieNameENGByMovieId(string? Id);
        public Schedule GetScheduleById(int? id);
        public ScheduleSeat GetScheduleSeatById(string? id);
        public Task<string> GetStatusSeatById(string? ScheduleSeatId);
        public Task<DateTime> GetReserveUntilById(string? ScheduleSeatId);
        public Task<bool> AddScheduleSeatAsync(int scheduleId,string movieId,int roomid);
        public Task<List<ScheduleSeat>> GetAllSeatsSelected(RequestDTOSeats request, string AccountId);
    }
}
