using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI.Controllers;
using WebAPI.Exception;
using WebAPI.Services;
using WebAPI.Services.DTO.Request;
using WebAPI.Services.DTO.Response;
using Xunit;

namespace WebAPI.Tests
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthenticateService> _mockAuthService;
        private readonly AuthController _authController;
        private readonly Mock<HttpContext> _mockHttpContext;
        private readonly Mock<IResponseCookies> _mockResponseCookies;

        public AuthControllerTests()
        {
            _mockAuthService = new Mock<IAuthenticateService>();
            _authController = new AuthController(_mockAuthService.Object);

            _mockHttpContext = new Mock<HttpContext>();
            _mockResponseCookies = new Mock<IResponseCookies>();

            _mockHttpContext.Setup(ctx => ctx.Response.Cookies).Returns(_mockResponseCookies.Object);
            _authController.ControllerContext = new ControllerContext
            {
                HttpContext = _mockHttpContext.Object
            };
        }

        [Fact]
        public async Task Login_ValidRequest_SetsAccessTokenCookie()
        {
            // Arrange
            var request = new RequestDTOLogin { Username = "testuser", Password = "password" };
            var tokenResponse = new ResponseDTOToken { AccessToken = "access_token", RefreshToken = "refresh_token" };
            _mockAuthService.Setup(service => service.Login(request)).ReturnsAsync(tokenResponse);

            // Act
            var result = await _authController.Login(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(tokenResponse, okResult.Value);

            // Kiểm tra xem cookie đã được thiết lập chưa
            _mockResponseCookies.Verify(cookies => cookies.Append("accessToken", tokenResponse.AccessToken, It.IsAny<CookieOptions>()), Times.Once);
            _mockResponseCookies.Verify(cookies => cookies.Append("refreshToken", tokenResponse.RefreshToken, It.IsAny<CookieOptions>()), Times.Once);
        }

        [Fact]
        public async Task Logout_AuthenticatedUser_ClearsCookies()
        {
            // Arrange
            var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "testuser")
            }, "mock"));
            _mockHttpContext.Setup(ctx => ctx.User).Returns(claimsPrincipal);

            _mockAuthService.Setup(service => service.RemoveRefreshToken("testuser")).Returns(Task.CompletedTask);

            // Act
            var result = await _authController.Logout();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Logout successfull", okResult.Value);

            // Kiểm tra xem cookie đã được xóa chưa
            _mockResponseCookies.Verify(cookies => cookies.Append("accessToken", "", It.Is<CookieOptions>(opts => opts.Expires <= DateTimeOffset.UtcNow)), Times.Once);
            _mockResponseCookies.Verify(cookies => cookies.Append("refreshToken", "", It.Is<CookieOptions>(opts => opts.Expires <= DateTimeOffset.UtcNow)), Times.Once);
        }
    }
}
