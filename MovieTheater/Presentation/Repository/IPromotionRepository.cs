using WebAPI.Entity;

namespace WebAPI.Repository
{
    public interface IPromotionRepository : IGenericRepository<Promotion>
    {
        Task<IEnumerable<Promotion>> GetAllPromotionsAsync(int page, int pageSize, string? searchTerm);
        Task<Promotion?> GetPromotionByIdAsync(int id);
        Task AddPromotionAsync(Promotion promotion);
        Task UpdatePromotionAsync(Promotion promotion);
        Task DeletePromotionAsync(int id);
        Task<int> GetTotalCountAsync(string? searchTerm);
    }
}
