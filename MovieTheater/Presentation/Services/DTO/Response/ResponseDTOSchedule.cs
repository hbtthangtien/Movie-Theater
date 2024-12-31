namespace WebAPI.Services.DTO.Response;

public class ResponseDTOSchedule
{
    public string MovieId { get; set; } = null!;
    public string? MovieNameEnglish { get; set; }
    public int ScheduleId { get; set; }
    public string? ScheduleTime { get; set; }

    public DateOnly? MovieScheduleDate { get; set; }
    public int? CinemaRoomId { get; set; }
    public string? SmallImage { get; set; }
    public string? CinemeRoomName { get; set; }
}