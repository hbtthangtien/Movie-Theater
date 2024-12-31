using WebAPI.Entity;

namespace WebAPI.Repository
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        public List<Employee> GetEmployeeForManagement(string? search, int page = 1);
        
        public Employee GetEmployeeById(string id);

    }
}
