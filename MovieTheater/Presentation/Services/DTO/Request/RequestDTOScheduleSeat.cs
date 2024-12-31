namespace WebAPI.Services.DTO.Request
{
    public class RequestDTOScheduleSeat
    {
        public string ScheduleSeatId { get; set; } 
        public string? seatColumn { get; set; } 
        public int? seatRow { get; set; }
        public seatTypeDTO? seatType { get; set; }
        public string? seatStatus { get; set; }
        public DateTime? ReverseUntil { get; set; }
    }
}