using WebAPI.Entity;

namespace WebAPI.Services.DTO.Response
{
    public class ResponseDTOSeats
    {
        public string? ScheduleSeatId {  get; set; }

        public string? MovieId { get; set; }

        public string? ScheduleId {  get; set; }

        public string? SeatId { get; set;}

        public string? SeatColumn { get; set; }

        public int? SeatRow { get; set; }

        public string? SeatStatus { get; set; } 
        public seatTypeDTO? SeatType { get; set; }
        public DateTime? ReservedUntil { get; set; }

        public string? AccountId {  get; set; }
    }
}
