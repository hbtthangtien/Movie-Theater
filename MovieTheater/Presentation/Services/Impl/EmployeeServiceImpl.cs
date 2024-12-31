using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using System.Net;
using WebAPI.Entity;
using WebAPI.Exception;
using WebAPI.Repository;
using WebAPI.Services.DTO.Request;
using WebAPI.Services.DTO.Response;

namespace WebAPI.Services.Impl
{
    public class EmployeeServiceImpl : GenericServices, IEmployeeService
    {
        private IValidService _validService;
        private MovieTheaterContext _movieTheaterContext;
        private IDbContextTransaction _transaction;

        public EmployeeServiceImpl(IMapper mapper, IUnitOfWork unitOfWork, IValidService validService, MovieTheaterContext movieTheaterContext) : base(mapper, unitOfWork)
        {
            _validService = validService;
            _movieTheaterContext = movieTheaterContext;
        }

        public async Task<ResponseDTOApi> DeleteEmployee(string employeeId)
        {
            ResponseDTOApi response = new ResponseDTOApi();

            if (string.IsNullOrWhiteSpace(employeeId))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.StatusMessage = "Delete failed";
                response.errors.Add("EmployeeId is required.");
                return response;
            }
            try
            {
                await BeginTransaction();
                var employee = _context.Employees.GetEmployeeById(employeeId);
                    
                if (employee == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.StatusMessage = "Delete failed";
                    response.errors.Add("Employee not found.");
                    return response;
                }

                employee.Account.Status = 0; 
                _context.Employees.Update(employee);

                await _context.CommitAsync();
                await CommitTransaction();

                response.StatusCode = HttpStatusCode.OK;
                response.StatusMessage = "Employee deleted successfully.";
            }
            catch (System.Exception ex)
            {
                await RollbackTransaction();
                var message1 = ex.InnerException;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.StatusMessage = "Delete operation failed.";
                response.errors = ((message1 != null) ? message1.Message : ex.Message).Split('\\').ToList();
            }

            return response;
        }

        public async Task<ResponseDTOApi> EditEmployee(string employeeId, RequestDTOEmployeeManagement employeeDTO)
        {
            ResponseDTOApi response = new ResponseDTOApi();

            try
            {
                if (!string.IsNullOrEmpty(employeeDTO.Email) && !_validService.IsEmailValid(employeeDTO.Email!))
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.errors.Add("Email must match the format and contain at least 1 domain.");
                    return response;
                }
                if (employeeDTO.DateOfBirth != null && !_validService.IsAgeValid((DateOnly)employeeDTO.DateOfBirth!))
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.errors.Add("User's age is not valid.");
                    return response;
                }

                var existingEmployee = _context.Employees.GetEmployeeById(employeeId);

                if (existingEmployee == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.StatusMessage = "Employee not found.";
                    return response;
                }

                await BeginTransaction();

                if (!string.IsNullOrEmpty(employeeDTO.Fullname))
                    existingEmployee.Account.Fullname = employeeDTO.Fullname;

                if (!string.IsNullOrEmpty(employeeDTO.Email))
                    existingEmployee.Account.Email = employeeDTO.Email;

                if (!string.IsNullOrEmpty(employeeDTO.PhoneNumber))
                    existingEmployee.Account.PhoneNumber = employeeDTO.PhoneNumber;

                if (!string.IsNullOrEmpty(employeeDTO.Address))
                    existingEmployee.Account.Address = employeeDTO.Address;

                if (employeeDTO.DateOfBirth != null)
                    existingEmployee.Account.DateOfBirth = employeeDTO.DateOfBirth;

                if (!string.IsNullOrEmpty(employeeDTO.Gender))
                    existingEmployee.Account.Gender = employeeDTO.Gender;

                _context.Employees.Update(existingEmployee);
                await _context.CommitAsync();

                await CommitTransaction();

                response.StatusCode = HttpStatusCode.OK;
                response.StatusMessage = "Employee updated successfully.";
            }
            catch (System.Exception ex)
            {
                await RollbackTransaction();
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.StatusMessage = "Edit operation failed.";
                response.errors = new List<string> { ex.InnerException?.Message ?? ex.Message };
            }

            return response;
        }

        public List<ResponseDTOEmployeeManagement> GetEmployee(string? search, int page = 1)
        {
            try
            {
                var employees = _context.Employees.GetEmployeeForManagement(search, page)
                .Select(e => new ResponseDTOEmployeeManagement
                {
                    EmployeeId = e.EmployeeId,
                    AccountId = e.EmployeeId,
                    Username = e.Account.UserName,
                    Fullname = e.Account.Fullname,
                    DateOfBirth = e.Account.DateOfBirth,
                    Gender = e.Account?.Gender,
                    Email = e.Account.Email,
                    IdentityCard = e.Account.IdentityCard,
                    PhoneNumber = e.Account.PhoneNumber,
                    Address = e.Account.Address,
                    RegisterDate = e.Account.RegisterDate

                }).ToList();
                return employees;
            }
            catch(System.Exception e) {
                throw new ApplicationException(e.Message);
            }
            
        }

        private async Task BeginTransaction()
        {
            if (_transaction == null)
            {
                _transaction = await _movieTheaterContext.Database.BeginTransactionAsync();
            }
        }
        private async Task CommitTransaction()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
            }
        }
        private async Task RollbackTransaction()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
            }
        }
    }
}
