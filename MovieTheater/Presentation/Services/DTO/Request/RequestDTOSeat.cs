namespace WebAPI.Services.DTO.Request;

public class RequestDTOSeat
{
    public int SeatId { get; set; }

    public int? CinemaRoomId { get; set; }

    public string? SeatColunm { get; set; }

    public int? SeatRow { get; set; }

    public int? SeatStatus { get; set; }
    public int? seatType_id { get; set; }
}