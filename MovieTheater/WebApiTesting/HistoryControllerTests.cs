using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers;
using WebAPI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Migrations;

public class HistoryControllerTests
{
    private readonly Mock<IHistoryRepository> _mockHistoryService;
    private readonly HistoryController _controller;
    private readonly IInvoiceService _invoiceService;
    public HistoryControllerTests(IInvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
        _mockHistoryService = new Mock<IHistoryRepository>();
        _controller = new HistoryController(invoiceService);
    }

    

    [Fact]
    public async Task ClearUserHistory_ShouldReturnNoContent_WhenHistoryIsCleared()
    {
        // Arrange
        var userId = "1";
        _mockHistoryService.Setup(service => service.GetDeleteScript(userId))
            ;

        // Act
        var result = await _controller.ViewBookedTicketsHistory(userId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task ClearUserHistory_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = "99";
        _mockHistoryService.Setup(service => service.GetDeleteScript(userId))
            ;

        // Act
        var result = await _controller.ViewBookedTicketsHistory(userId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}