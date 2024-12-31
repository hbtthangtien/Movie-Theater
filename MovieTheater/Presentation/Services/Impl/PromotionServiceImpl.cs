using AutoMapper;
using WebAPI.Entity;
using WebAPI.Repository;
using WebAPI.Services.DTO;

namespace WebAPI.Services.Impl
{
    public class PromotionServiceImpl : GenericServices, IPromotionService
    {
        private readonly IPromotionRepository _repository;
        public PromotionServiceImpl(IMapper mapper, IUnitOfWork unitOfWork, IPromotionRepository repository) : base(mapper, unitOfWork)
        {
            _repository = repository;
        }
        public async Task<(IEnumerable<Promotion>, int)> GetPromotionsAsync(int page, int pageSize, string? searchTerm)
        {
            var promotions = await _repository.GetAllPromotionsAsync(page, pageSize, searchTerm);
            var totalCount = await _repository.GetTotalCountAsync(searchTerm);
            return (promotions, totalCount);
        }

        public async Task<Promotion?> GetPromotionByIdAsync(int id)
        {
            return await _repository.GetPromotionByIdAsync(id);
        }

        public async Task AddPromotionAsync(Promotion promotion)
        {
            ValidatePromotion(promotion);
            await _repository.AddPromotionAsync(promotion);
        }

        public async Task UpdatePromotionAsync(Promotion promotion)
        {
            ValidatePromotion(promotion);
            await _repository.UpdatePromotionAsync(promotion);
        }

        public async Task DeletePromotionAsync(int id)
        {
            await _repository.DeletePromotionAsync(id);
        }

        private void ValidatePromotion(Promotion promotion)
        {
            if (string.IsNullOrWhiteSpace(promotion.Title))
                throw new ArgumentException("Title is required.");

            if (promotion.StartTime >= promotion.EndTime)
                throw new ArgumentException("Start time must be earlier than end time.");

            if (promotion.DiscountLevel < 0 || promotion.DiscountLevel > 100)
                throw new ArgumentException("Discount level must be between 0 and 100.");
        }

        public async Task<IEnumerable<Promotion>> GetAllPromotionsAsync()
        {
            return await _repository.GetAllAsync();
        }
    }
}
