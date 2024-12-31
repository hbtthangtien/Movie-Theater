namespace WebAPI.Services.DTO
{
    public class PromotionDTO
    {
        public int? PromotionId { get; set; }
        public string? Detail { get; set; }
        public int? DiscountLevel { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Image { get; set; }
        public DateTime? StartTime { get; set; }
        public string? Title { get; set; }
    }
}
