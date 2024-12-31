using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers;
using WebAPI.Services;
using WebAPI.Services.DTO.Request;
using WebAPI.Services.DTO.Response;
using System.Collections.Generic;
using System.Linq;

namespace MovieTheater.Tests.Controllers
{
    public class EmployeeControllerTests
    {
        private readonly Mock<IEmployeeService> _mockEmployeeService;
        private readonly Mock<IAccountService> _mockAccountService;
        private readonly EmployeeController _controller;

        public EmployeeControllerTests()
        {
            _mockEmployeeService = new Mock<IEmployeeService>();
            _mockAccountService = new Mock<IAccountService>();

            _controller = new EmployeeController(_mockEmployeeService.Object, _mockAccountService.Object);
        }

        [Fact]
        public void GetEmployees_ShouldReturnOk_WhenEmployeesExist()
        {
            // Arrange
            var search = "John";
            int page = 1;

            var employees = new List<ResponseDTOEmployeeManagement>
            {
                new ResponseDTOEmployeeManagement {  AccountId= "1", Username = "John Doe" },
                new ResponseDTOEmployeeManagement { AccountId = "2", Fullname = "Jane Smith" }
            };

            _mockEmployeeService.Setup(service => service.GetEmployee(search, page))
                .Returns(employees);

            // Act
            var result = _controller.GetEmployees(search, page);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedEmployees = Assert.IsType<List<ResponseDTOEmployeeManagement>>(okResult.Value);
            Assert.Equal(2, returnedEmployees.Count);
        }

        [Fact]
        public void GetEmployees_ShouldReturnNotFound_WhenNoEmployeesExist()
        {
            // Arrange
            var search = "Nonexistent";
            int page = 1;

            _mockEmployeeService.Setup(service => service.GetEmployee(search, page))
                .Returns(new List<ResponseDTOEmployeeManagement>());

            // Act
            var result = _controller.GetEmployees(search, page);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
        
        

        [Fact]
        public void DeleteEmployee_ShouldReturnNoContent_WhenEmployeeExists()
        {
            // Arrange
            var employeeId = "1";
            ResponseDTOApi response = new ResponseDTOApi();
            _mockEmployeeService.Setup(service => service.DeleteEmployee(employeeId))
                ;
                
            // Act
            var result = _controller.DeleteEmployee(employeeId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void DeleteEmployee_ShouldReturnNotFound_WhenEmployeeDoesNotExist()
        {
            // Arrange
            String employeeId = "999";

            _mockEmployeeService.Setup(service => service.DeleteEmployee(employeeId))
                ;

            // Act
            var result = _controller.DeleteEmployee(employeeId);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
    }
}
