namespace WebAPI.Services.DTO.Response
{
    public class ResponseDTOHistory
    {
        public string InvoiceId { get; set; } = null!;
        public string? MovieName { get; set; }
        public DateTime? BookingDate { get; set; }
        public decimal? TotalMoney { get; set; }
        public string Status { get; set; }
        public double? AddScore { get; set; }
        public double? UseScore { get; set; }
        public string? AccountId { get; set; }
        public string? Image { get; set; }
        public string? ScheduleShowTime { get; set; }
        public string userName { get; set; }
        public string? Seat { get; set; }
        public string? cinema_room_name { get; set; }
        public double? Score { get; set; }
    }
}
