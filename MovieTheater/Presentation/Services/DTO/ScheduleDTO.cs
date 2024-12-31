namespace WebAPI.Services.DTO.Response
{
    public class ScheduleDTO
    {
        public int ScheduleId { get; set; }

        public string? ScheduleTime { get; set; }
        
        public string MovieId { get; set; } = null!;
        
        public DateOnly? MovieScheduleDate { get; set; }
    }
}