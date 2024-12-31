using WebAPI.Services.DTO.Request;

namespace WebAPI.Services;

public interface ISeatServices
{
    public Task<bool> AddSeat(List<RequestDTOSeat> seats);
    public Task<bool> UpdateSeat(List<RequestDTOSeat> seat);
    public Task<bool> DeleteSeat(List<int> seatIds);
    
    public Task<List<RequestDTOSeat>> GetAllSeatByCinemaID(int cinemaID);
}