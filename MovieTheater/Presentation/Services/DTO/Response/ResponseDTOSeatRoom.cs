namespace WebAPI.Services.DTO.Response;

public class ResponseDTOSeatRoom
{
    public string? CinemeRoomName { get; set; }
    public List<ResponseDTOScheduleSeat> ScheduleSeats { get; set; }
}