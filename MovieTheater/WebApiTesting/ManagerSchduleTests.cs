using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers;
using WebAPI.Services;
using WebAPI.Services.DTO.Request;
using System.Threading.Tasks;

public class ManagerSchduleTests
{
    private readonly Mock<IScheduleService> _mockScheduleService;
    private readonly Mock<IMovieRomeServices> _mockMovieRoomService;
    private readonly Mock<IMovieTypeServices> _mockMovieTypeService;
    private readonly Mock<IMovieScheduleService> _mockMovieScheduleService;
    private readonly Mock<IMovieService> _mockMovieService;
    private readonly Mock<IScheduleSeatService> _mockScheduleSeatService;
    private readonly ManagerSchdule _controller;

    public ManagerSchduleTests()
    {
        _mockScheduleService = new Mock<IScheduleService>();
        _mockMovieRoomService = new Mock<IMovieRomeServices>();
        _mockMovieTypeService = new Mock<IMovieTypeServices>();
        _mockMovieScheduleService = new Mock<IMovieScheduleService>();
        _mockMovieService = new Mock<IMovieService>();
        _mockScheduleSeatService = new Mock<IScheduleSeatService>();

        _controller = new ManagerSchdule(
            _mockMovieRoomService.Object,
            _mockMovieTypeService.Object,
            _mockMovieScheduleService.Object,
            _mockMovieService.Object,
            _mockScheduleService.Object,
            _mockScheduleSeatService.Object
        );
    }
    //
    // [Fact]
    // public async Task AddMoviesettime_ShouldReturnOk_WhenScheduleIsAddedSuccessfully()
    // {
    //     // Arrange
    //     var movieSchedule = new RequestDTOSchedule
    //     {
    //         movieID = "1",
    //         cinemaRoomID = 1,
    //         ScheduleTime = "6 AM : 9 AM",
    //         
    //     };
    //
    //     _mockScheduleService.Setup(service => service.addSchedule(movieSchedule))
    //         .ReturnsAsync(true);
    //
    //     // Act
    //     var result = await _controller.AddMoviesettime(movieSchedule);
    //
    //     // Assert
    //     var okResult = Assert.IsType<OkObjectResult>(result);
    //     Assert.True((bool)okResult.Value);
    // }

    // [Fact]
    // public async Task AddMoviesettime_ShouldReturnOk_WhenScheduleAdditionFails()
    // {
    //     // Arrange
    //     var movieSchedule = new RequestDTOSchedule
    //     {
    //         movieID = "1",
    //         cinemaRoomID = 1,
    //         ScheduleTime = "6 AM : 9 AM",
    //     };
    //
    //     _mockScheduleService.Setup(service => service.addSchedule(movieSchedule))
    //         .ReturnsAsync(false);
    //
    //     // Act
    //     var result = await _controller.AddMoviesettime(movieSchedule);
    //
    //     // Assert
    //     var okResult = Assert.IsType<OkObjectResult>(result);
    //     Assert.False((bool)okResult.Value);
    // }

    [Fact]
    public async Task RemoveSchedule_ShouldReturnOk_WhenScheduleIsRemovedSuccessfully()
    {
        // Arrange
        int scheduleId = 1;
        _mockScheduleService.Setup(service => service.deleteSchedule(scheduleId))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.RemoveSchedule(scheduleId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.True((bool)okResult.Value);
    }

    [Fact]
    public async Task RemoveSchedule_ShouldReturnOk_WhenScheduleRemovalFails()
    {
        // Arrange
        int scheduleId = 1;
        _mockScheduleService.Setup(service => service.deleteSchedule(scheduleId))
            .ReturnsAsync(false);

        // Act
        var result = await _controller.RemoveSchedule(scheduleId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.False((bool)okResult.Value);
    }
}
