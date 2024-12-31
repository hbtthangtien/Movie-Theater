using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebAPI.Services;
using WebAPI.Services.DTO.Request;
using WebAPI.Services.DTO.Response;

namespace WebAPI.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private IAccountService _accountService;

        public EmployeeController(IEmployeeService employeeService, IAccountService accountService) {
            _employeeService = employeeService;
            _accountService = accountService;
        }

        [HttpGet("admin/emloyees")]
        public IActionResult GetEmployees(string search="", int page = 1) {
            try
            {
                if (page < 1)
                    return BadRequest("Page number must be greater than 0.");
                var employees = _employeeService.GetEmployee(search, page);
                if (employees == null || !employees.Any())
                {
                    return NotFound("No Employee !!");
                } else return Ok(employees);

            } catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost("admin/employees")]
        public async Task<IActionResult> AddNewEmployee(RequestDTORegister request)
        {
            if (request == null)
            {
                return BadRequest(new ResponseDTOApi
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    StatusMessage = "Data is null"
                });
            }
            else
            {
                var response = await _accountService.CreateAccountEmployee(request);
                return StatusCode((int)response.StatusCode, response);

            }
        }

        [HttpPut("admin/employees")]
        public async Task<IActionResult> EditEmployee(string employeeId, RequestDTOEmployeeManagement employeeDTO)
        {
            try
            {
                var response = await _employeeService.EditEmployee(employeeId, employeeDTO);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return Ok(new { message = response.StatusMessage });
                }

                return StatusCode((int)response.StatusCode, response);
            }catch (System.Exception e) {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("admin/emoloyees")]
        public async Task<IActionResult> DeleteEmployee(string employeeId)
        {
            var response = await _employeeService.DeleteEmployee(employeeId);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(new { message = response.StatusMessage });
            }

            return StatusCode((int)response.StatusCode, response);
        }
    }
}
