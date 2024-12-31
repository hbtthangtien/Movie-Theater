using WebAPI.Entity;
using WebAPI.Services.DTO;

namespace WebAPI.Services
{

    public interface IPromotionService 
    {
        Task<(IEnumerable<Promotion>, int)> GetPromotionsAsync(int page, int pageSize, string? searchTerm);
        Task<Promotion?> GetPromotionByIdAsync(int id);
        Task AddPromotionAsync(Promotion promotion);
        Task UpdatePromotionAsync(Promotion promotion);
        Task DeletePromotionAsync(int id);
        Task<IEnumerable<Promotion>> GetAllPromotionsAsync();
    }
}
