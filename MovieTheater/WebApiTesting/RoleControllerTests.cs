using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers;
using WebAPI.Services;
using WebAPI.Services.DTO.Request;
using System.Threading.Tasks;
using WebAPI.Repository;

public class RoleControllerTests
{
    private readonly Mock<IQRCodeServicee> _mockQRCodeService;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly RoleController _controller;

    public RoleControllerTests()
    {
        _mockQRCodeService = new Mock<IQRCodeServicee>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        _controller = new RoleController(_mockUnitOfWork.Object, _mockQRCodeService.Object);
    }

    [Fact]
    public async Task Get_ShouldReturnOkResult_WhenQRCodeGeneratedSuccessfully()
    {
        // Arrange
        var request = new RequestDTOQRCode { AddInfor = "Test Data" };

        _mockQRCodeService.Setup(service => service.GenerateVietQRCodeAsync(request))
            ;

        // Act
        var result = await _controller.Get(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var qrCodeUrl = Assert.IsType<string>(okResult.Value);
        Assert.Equal("http://example.com/qrcode.png", qrCodeUrl);
    }

    [Fact]
    public async Task Get_ShouldReturnBadRequest_WhenExceptionOccurs()
    {
        // Arrange
        var request = new RequestDTOQRCode { AddInfor = "Test Data" };

        _mockQRCodeService.Setup(service => service.GenerateVietQRCodeAsync(request))
            .ThrowsAsync(new System.Exception("Error generating QR code."));

        // Act
        var result = await _controller.Get(request);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var errorMessage = Assert.IsType<string>(badRequestResult.Value);
        Assert.Equal("Error generating QR code.", errorMessage);
    }
}