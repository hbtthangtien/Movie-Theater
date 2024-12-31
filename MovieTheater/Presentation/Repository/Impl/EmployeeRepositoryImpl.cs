using Microsoft.EntityFrameworkCore;
using WebAPI.Entity;
using WebAPI.Exception;

namespace WebAPI.Repository.Impl
{
    public class EmployeeRepositoryImpl : GenericRepositoryImpl<Employee>, IEmployeeRepository
    {
        public static int pageNumber { get; set; } = 5;
        public EmployeeRepositoryImpl(MovieTheaterContext context) : base(context)
        {
        }

        public List<Employee> GetEmployeeForManagement(string? search, int page = 1)
        {
            var employees = _context.Employees.Include(e => e.Account).Where(e => e.Account.Status == 1).AsQueryable();
            if(string.IsNullOrEmpty(search) != true)
            {
                employees = employees.Where(e => e.Account.Status == 1 && e.Account.Fullname.Contains(search) || e.Account.UserName.Contains(search)) ?? throw new NotFoundException("No Employee !!");
            }
            employees = employees.Skip((page - 1) * pageNumber).Take(pageNumber);
            return employees.ToList();
        }

        public Employee GetEmployeeById(string id)
        {
            var employee = _context.Employees.Include(e => e.Account).FirstOrDefault(e => e.EmployeeId == id);

            return employee;
        }
    }
}
