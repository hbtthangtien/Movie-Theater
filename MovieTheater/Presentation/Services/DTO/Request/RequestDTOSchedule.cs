namespace WebAPI.Services.DTO.Request;

public class RequestDTOSchedule
{
    public int ScheduleId { get; set; }

    public string? ScheduleTime { get; set; }

    public DateOnly? MovieScheduleDate { get; set; }
    public int cinemaRoomID { get; set; }
    public string movieID { get; set; }
}