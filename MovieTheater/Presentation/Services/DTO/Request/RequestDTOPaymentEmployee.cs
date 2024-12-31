namespace WebAPI.Services.DTO.Request
{
    public class RequestDTOPaymentEmployee
    {
        public string? MovieName { get; set; }
        public string? CinemaRoom { get; set; }
        public virtual ICollection<RequestDTOPayment.RequestDTOScheduleSeat> ScheduleSeats { get; set; } = new List<RequestDTOPayment.RequestDTOScheduleSeat>();
        public DateOnly? ScheduleShow { get; set; }
        public string? ScheduleShowTime { get; set; }
        public string? invoiceMessage;
        public bool IsUseScore { get; set; } = false!;
        public double UseScore { get; set; } = 0!;
        public string? member {  get; set; } = string.Empty;
        public bool Confirm {  get; set; }
        public string? InvoiceId { get; set; }
        
    }
}
