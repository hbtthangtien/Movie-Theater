using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Entity;

public partial class ScheduleSeat
{
    public string ScheduleSeatId { get; set; } = null!;
    public string? MovieId { get; set; }
    public int? ScheduleId { get; set; }
    public int? SeatId { get; set; }
    public string? SeatColumn { get; set; }
    public int? SeatRow { get; set; }
    public string? SeatStatus { get; set; }
    public int? seatType_id { get; set; }
    public string? AccountId { get; set; }
    public DateTime? ReservedUntil { get; set; }
    public virtual Movie? Movie { get; set; }
    public virtual Schedule? Schedule { get; set; }
    public virtual Seat? Seat { get; set; }
    public virtual TypeSeat? TypeSeat { get; set; }
    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual ApplicationUser? Account {  get; set; }
}
