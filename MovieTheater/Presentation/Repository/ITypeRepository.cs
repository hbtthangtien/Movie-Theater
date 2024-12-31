using Type = WebAPI.Entity.Type;

namespace WebAPI.Repository
{
    public interface ITypeRepository : IGenericRepository<Type>
    {
        public List<Type> GetAllTypes();
    }
}
