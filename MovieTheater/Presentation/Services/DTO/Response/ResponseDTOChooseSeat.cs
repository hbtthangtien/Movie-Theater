using WebAPI.Services.DTO.Request;

namespace WebAPI.Services.DTO.Response;

public class ResponseDTOChooseSeat
{
    public String movieName { get; set; }
    public String cinemaRoom { get; set; }
    public List<RequestDTOScheduleSeat> scheduleSeats { get; set; }
    public DateOnly scheduleShow { get; set; }
    public String? scheduleShowTime { get; set; }

    public string? AccountId { get; set; }

}