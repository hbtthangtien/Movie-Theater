using System;
using System.Collections.Generic;

namespace WebAPI.Entity;

public partial class Invoice
{
    public string InvoiceId { get; set; } = null!;
    public double? AddScore { get; set; }

    public DateTime? BookingDate { get; set; }

    public string? MovieName { get; set; }

    public DateOnly? ScheduleShow { get; set; }

    public string? ScheduleShowTime { get; set; }

    public string? Status { get; set; }

    public decimal? TotalMoney { get; set; }

    public double? UseScore { get; set; }

    public string? Seat { get; set; }
    public string? cinema_room_name { get; set; }
    public string? AccountId { get; set; }
    public virtual ApplicationUser? Account { get; set; }
}
