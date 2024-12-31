public class RequestDTOPayment
{
    public string? MovieName { get; set; }
    public string? CinemaRoom { get; set; }
    public virtual ICollection<RequestDTOScheduleSeat> ScheduleSeats { get; set; } = new List<RequestDTOScheduleSeat>();
    public DateOnly? ScheduleShow { get; set; }
    public string? ScheduleShowTime { get; set; }
    public string? invoiceMessage { get; set; }
    public string? InvoiceId { get; set; }
    public double UseScore { get; set; } = 0!;

    // Internal nested class, accessible within the assembly
    public class RequestDTOScheduleSeat
    {
        public string? ScheduleSeatId { get; set; }
        public string? SeatColumn { get; set; }
        public int SeatRow { get; set; }
        public RequestDTOSeatType Type { get; set; }
        public DateTime? ReserveUntil { get; set; }
        public class RequestDTOSeatType
        {
            public string? Name { get; set; }
            public double Price { get; set; } = 0!;
        }
    }
}
