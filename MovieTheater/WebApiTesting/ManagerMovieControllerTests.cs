// using Xunit;
// using Moq;
// using Microsoft.AspNetCore.Mvc;
// using WebAPI.Controllers;
// using WebAPI.Services;
// using WebAPI.Services.DTO;
// using System.Collections.Generic;
// using System.Threading.Tasks;
//
// public class ManagerMovieControllerTests
// {
//     private readonly Mock<IMovieService> _mockMovieService;
//     private readonly ManagerMovie _controller;
//     private readonly IMovieScheduleService _movieScheduleService;
//     private readonly IMovieService _movieService;
//     private readonly IScheduleService _scheduleService;
//     private readonly IScheduleSeatService _scheduleSeatService;
//     private readonly IMovieTypeServices _movieTypeServices;
//     private readonly IMovieRomeServices _movieroomService;
//     public ManagerMovieControllerTests(Mock<IMovieService> mockMovieService,IMovieRomeServices movieroomService,IMovieTypeServices movieTypeServices,
//         IMovieScheduleService movieScheduleService, IMovieService movieService,
//         IScheduleService scheduleService,IScheduleSeatService scheduleSeatService)
//     {
//         _mockMovieService = new Mock<IMovieService>();
//         _controller = new ManagerMovie(movieroomService, movieTypeServices, movieScheduleService, movieService, scheduleService, scheduleSeatService);
//     }
//
//     [Fact]
//     public async Task GetMoviesPaged_ShouldReturnOkResult_WithPagedData()
//     {
//         // Arrange
//         var movies = new List<MovieDTO>
//         {
//             new MovieDTO { MovieId = "1", MovieNameEnglish = "Movie 1" },
//             new MovieDTO { MovieId = "2", MovieNameEnglish = "Movie 2" }
//         };
//
//         _mockMovieService.Setup(service => service.GetMoviesPagedAsync("", 1, 2,0))
//             .ReturnsAsync((movies, 10, 5));
//
//         // Act
//         var result = await _controller.GetMoviesPaged(search: "", page: 1, pageSize: 2);
//
//         // Assert
//         var okResult = Assert.IsType<OkObjectResult>(result);
//         var data = Assert.IsType<dynamic>(okResult.Value);
//
//         Assert.Equal(10, data.TotalCount);
//         Assert.Equal(5, data.TotalPages);
//         Assert.Equal(1, data.CurrentPage);
//         Assert.Equal(2, data.PageSize);
//         Assert.Equal(2, ((List<MovieDTO>)data.Movies).Count);
//     }
//
//     [Fact]
//     public async Task GetMoviesPaged_ShouldReturnBadRequest_WhenPageExceedsTotalPages()
//     {
//         // Arrange
//         _mockMovieService.Setup(service => service.GetMoviesPagedAsync("", 10, 5,0))
//             .ThrowsAsync(new ArgumentException("Page 10 exceeds the total number of pages 5."));
//
//         // Act
//         var result = await _controller.GetMoviesPaged(search: "", page: 10, pageSize: 5);
//
//         // Assert
//         var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
//         Assert.Equal("Page 10 exceeds the total number of pages 5.", badRequestResult.Value);
//     }
//
//     [Fact]
//     public async Task GetMoviesPaged_ShouldReturnOkResult_WithEmptyData()
//     {
//         // Arrange
//         _mockMovieService.Setup(service => service.GetMoviesPagedAsync("", 1, 5,0))
//             .ReturnsAsync((new List<MovieDTO>(), 0, 0));
//
//         // Act
//         var result = await _controller.GetMoviesPaged(search: "", page: 1, pageSize: 5);
//
//         // Assert
//         var okResult = Assert.IsType<OkObjectResult>(result);
//         var data = Assert.IsType<dynamic>(okResult.Value);
//
//         Assert.Equal(0, data.TotalCount);
//         Assert.Equal(0, data.TotalPages);
//         Assert.Empty((List<MovieDTO>)data.Movies);
//     }
// }
