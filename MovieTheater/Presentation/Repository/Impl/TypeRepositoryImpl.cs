using WebAPI.Entity;
using Type = System.Type;

namespace WebAPI.Repository.Impl
{
    public class TypeRepositoryImpl : GenericRepositoryImpl<Type>, ITypeRepository
    {
        public TypeRepositoryImpl(MovieTheaterContext context) : base(context)
        {
        }


        public List<Entity.Type> GetAllTypes()
        {
            var result = _context.Types.ToList();
            return result;
        }

        

        public Task<Entity.Type> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Entity.Type>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(Entity.Type entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Entity.Type entity)
        {
            throw new NotImplementedException();
        }

        public void Remove(Entity.Type entity)
        {
            throw new NotImplementedException();
        }
    }
}
