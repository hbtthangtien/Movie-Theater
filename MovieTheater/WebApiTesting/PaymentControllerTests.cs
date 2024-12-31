// using Xunit;
// using Moq;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Http;
// using WebAPI.Controllers;
// using WebAPI.Services;
// using WebAPI.Services.DTO.Request;
// using WebAPI.Services.DTO.Response;
// using System.Collections.Generic;
// using System.Security.Claims;
// using System.Threading.Tasks;
//
// public class PaymentControllerTests
// {
//     private readonly Mock<IPaymentService> _mockPaymentService;
//     private readonly PaymentController _controller;
//
//     public PaymentControllerTests()
//     {
//         _mockPaymentService = new Mock<IPaymentService>();
//
//         _controller = new PaymentController(_mockPaymentService.Object);
//
//         // Mock HttpContext với User
//         var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
//         {
//             new Claim(ClaimTypes.Name, "test_user")
//         }, "mock"));
//
//         var httpContext = new DefaultHttpContext
//         {
//             User = user
//         };
//
//         _controller.ControllerContext = new ControllerContext
//         {
//             HttpContext = httpContext
//         };
//     }
//     
//
//     [Fact]
//     public async Task CallBack_ShouldReturnBadRequest_WhenExceptionOccurs()
//     {
//         // Arrange
//         var query = new QueryCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>());
//         _controller.ControllerContext.HttpContext.Request.Query = query;
//
//         _mockPaymentService.Setup(service => service.ExecutePayment(query, "test_user"))
//             .ThrowsAsync(new System.Exception("An error occurred."));
//
//         // Act
//         var result = await _controller.CallBack();
//
//         // Assert
//         var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
//         var response = Assert.IsType<ResponseDTOApi>(badRequestResult.Value);
//         Assert.Equal("An error occurred.", response.StatusMessage);
//     }
//
//     [Fact]
//     public async Task GenerateUrlPayment_ShouldReturnOk_WhenPaymentGeneratedSuccessfully()
//     {
//         // Arrange
//         var request = new RequestDTOPayment { UseScore = 100 };
//         _mockPaymentService.Setup(service => service.CreatePaymentAsync(request, It.IsAny<HttpContext>(), "test_user"))
//             .ReturnsAsync("http://payment.url");
//
//         // Act
//         var result = await _controller.GenerateUrlPayment(request);
//
//         // Assert
//         var okResult = Assert.IsType<OkObjectResult>(result);
//         var response = Assert.IsType<dynamic>(okResult.Value);
//         Assert.Equal("http://payment.url", response.vnp_UrlPayment);
//     }
//
//     [Fact]
//     public async Task GenerateUrlPayment_ShouldReturnUnauthorized_WhenUserNotAuthenticated()
//     {
//         // Arrange
//         _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(); // Không có user
//
//         var request = new RequestDTOPayment { UseScore = 100 };
//
//         // Act
//         var result = await _controller.GenerateUrlPayment(request);
//
//         // Assert
//         var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
//         var response = Assert.IsType<ResponseDTOApi>(unauthorizedResult.Value);
//         Assert.Equal("User is not authenticated!!!", response.StatusMessage);
//     }
//     
//
//     [Fact]
//     public async Task ConfirmPaymentEmployee_ShouldReturnBadRequest_WhenExceptionOccurs()
//     {
//         // Arrange
//         var request = new RequestDTOPaymentEmployee { UseScore = 100 };
//         _mockPaymentService.Setup(service => service.ComfirmPaymentForEmployee(request, "test_user"))
//             .Throws(new System.Exception("An error occurred."));
//
//         // Act
//         var result = await _controller.ConfirmPaymentEmployee(request);
//
//         // Assert
//         var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
//         var response = Assert.IsType<ResponseDTOApi>(badRequestResult.Value);
//         Assert.Equal("An error occurred.", response.StatusMessage);
//     }
// }
