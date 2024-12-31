using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebAPI.Entity;

namespace WebAPI.Repository.Impl
{
    public class GenericRepositoryImpl<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {

        protected readonly MovieTheaterContext _context;

        private readonly DbSet<TEntity> _dbSet;
        public GenericRepositoryImpl(MovieTheaterContext context)
        {
            if (_context == null)
            {
                _context = context;
            }
            _dbSet = _context.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(string id)
        {
            return await _dbSet.FindAsync(id);
        }

        public void Remove(TEntity entity)
        {
             _dbSet.Remove(entity);
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }
    }
}
