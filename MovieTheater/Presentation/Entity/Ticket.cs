using System;
using System.Collections.Generic;

namespace WebAPI.Entity;

public partial class Ticket
{
    public int TicketId { get; set; }
    public decimal? Price { get; set; }
    public string? movie_name { get; set; }
    public string? cinema_room_name { get; set; }
    public string? schedule_show_time {  get; set; }
    public DateOnly? schedule_show {  get; set; }
    public string? AccountId { get; set; }

    public string? ScheduleSeatId { get; set; }
    public virtual ScheduleSeat? ScheduleSeat { get; set; }
    public virtual ApplicationUser? Account { get; set; }
    public int? TicketType { get; set; }
}
