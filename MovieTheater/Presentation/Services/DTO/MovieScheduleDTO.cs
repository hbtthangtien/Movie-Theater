using WebAPI.Entity;

namespace WebAPI.Services.DTO
{
    public class MovieScheduleDTO
    {
        public string? MovieId { get; set; } = null!;

        public int? ScheduleId { get; set; }

        public DateOnly? MovieScheduleDate { get; set; }
    }
}