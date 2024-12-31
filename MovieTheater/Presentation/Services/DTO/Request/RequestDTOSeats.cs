namespace WebAPI.Services.DTO.Request
{
    public class RequestDTOSeats
    {
        public string? username { get; set; }

        public string? MovieId {  get; set; }

        public string? ScheduleSeatId { get; set; }

        public int? ScheduleId { get; set; }
    }
}
