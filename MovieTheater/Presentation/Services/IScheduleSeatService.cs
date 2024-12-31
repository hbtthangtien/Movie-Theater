using WebAPI.Services.DTO.Request;
using WebAPI.Services.DTO.Response;

namespace WebAPI.Services
{
    public interface IScheduleSeatService
    {
        public List<ResponseDTOScheduleSeat> GetScheduleSeats(String movieId, int schid);
        public List<ResponseDTOSeatRoom> GetListSeatRooms(string movieId, int schid);
        public Task<ResponseDTOChooseSeat> GetScheduleSeatsAsync(List<ResponseDTOScheduleSeat> scheduleSeats, string AccountId);
        public Task ReserveSeat(string ScheduleSeatId);
        public Task HoldSeat(string ScheduleSeatId, string AccountId);
        public Task SelectedSeat(string ScheduleSeatId);
        public Task<ResponseDTOChooseSeat> ExecuteSelectedSeat(List<ResponseDTOScheduleSeat> request,string username);
        public Task ReserveSelectedSeat(List<RequestDTOScheduleSeat> requests);
        
        public Task<List<ResponseDTOSeats>> GetAllSeatsSelected(RequestDTOSeats request);
    }
}