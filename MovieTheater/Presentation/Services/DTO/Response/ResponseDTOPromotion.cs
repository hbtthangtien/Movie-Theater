namespace WebAPI.Services.DTO.Response
{
    public class ResponseDTOPromotion
    {
        public string? Title { get; set; }
        public string? Detail { get; set; }
        public int? DiscountLevel { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Image { get; set; }
    }
}
