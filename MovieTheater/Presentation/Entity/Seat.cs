using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Entity;

public partial class Seat
{
    public int SeatId { get; set; }

    public int? CinemaRoomId { get; set; }

    public string? SeatColunm { get; set; }

    public int? SeatRow { get; set; }

    public string? SeatStatus { get; set; }
    public int? seatType_id { get; set; }

    public virtual CinemaRoom? CinemaRoom { get; set; }

    public virtual ICollection<ScheduleSeat> ScheduleSeats { get; set; } = new List<ScheduleSeat>();

    public virtual TypeSeat? TypeSeat { get; set; }
}
