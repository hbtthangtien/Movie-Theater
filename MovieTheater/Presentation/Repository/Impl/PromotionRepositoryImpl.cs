using WebAPI.Entity;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Repository.Impl
{
    public class PromotionRepositoryImpl : GenericRepositoryImpl<Promotion>, IPromotionRepository
    {
        private readonly MovieTheaterContext _context;
        public PromotionRepositoryImpl(MovieTheaterContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Promotion>> GetAllPromotionsAsync(int page, int pageSize, string? searchTerm)
        {
            return await _context.Promotions
                .Where(p => string.IsNullOrEmpty(searchTerm) || p.Title.Contains(searchTerm))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Promotion?> GetPromotionByIdAsync(int id)
        {
            return await _context.Promotions.FindAsync(id);
        }

        public async Task AddPromotionAsync(Promotion promotion)
        {
            promotion.PromotionId = 0;
            await _context.Promotions.AddAsync(promotion);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePromotionAsync(Promotion promotion)
        {
            _context.Promotions.Update(promotion);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePromotionAsync(int id)
        {
            var promotion = await GetPromotionByIdAsync(id);
            if (promotion != null)
            {
                _context.Promotions.Remove(promotion);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetTotalCountAsync(string? searchTerm)
        {
            return await _context.Promotions.CountAsync(p => string.IsNullOrEmpty(searchTerm) || p.Title.Contains(searchTerm));
        }
    }
}
