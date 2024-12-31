namespace WebAPI.Services.DTO.Response
{
    public class ResponseDTOScheduleSeat
    {
        public string ScheduleSeatId { get; set; } = null!;
        public string? MovieId { get; set; }
        public int? ScheduleId { get; set; }
        public int? SeatId { get; set; }
        public string? SeatColumn { get; set; }
        public int? SeatRow { get; set; }
        public string? SeatStatus { get; set; }
        public int? seatType_id { get; set; }       
        public string? CinemeRoomName { get; set; }
        public double price { get; set; }        
        public DateTime? ReverseUntil { get; set; }

    }
}