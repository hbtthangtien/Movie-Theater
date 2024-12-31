using WebAPI.Services.DTO.Request;
using WebAPI.Services.DTO.Response;

namespace WebAPI.Services
{
    public interface IEmployeeService
    {
        public List<ResponseDTOEmployeeManagement> GetEmployee(string? search, int page = 1);

        public Task<ResponseDTOApi> EditEmployee(string employeeId,RequestDTOEmployeeManagement employeeDTO);

        public Task<ResponseDTOApi> DeleteEmployee(string employeeId);
    }

}
