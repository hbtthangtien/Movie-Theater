using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers;
using WebAPI.Services;
using WebAPI.Entity;
using WebAPI.Services.DTO.Request;
using System.Collections.Generic;

namespace MovieTheater.Tests.Controllers
{
    public class CinemaRoomManagerTests
    {
        private readonly Mock<IMovieScheduleService> _mockMovieScheduleService;
        private readonly Mock<IMovieService> _mockMovieService;
        private readonly Mock<IScheduleService> _mockScheduleService;
        private readonly Mock<IScheduleSeatService> _mockScheduleSeatService;
        private readonly Mock<IMovieTypeServices> _mockMovieTypeServices;
        private readonly Mock<IMovieRomeServices> _mockMovieRoomService;
        private readonly Mock<ISeatServices> _mockSeatServices;
        private readonly cinemaRoomManager _controller;

        public CinemaRoomManagerTests()
        {
            _mockMovieScheduleService = new Mock<IMovieScheduleService>();
            _mockMovieService = new Mock<IMovieService>();
            _mockScheduleService = new Mock<IScheduleService>();
            _mockScheduleSeatService = new Mock<IScheduleSeatService>();
            _mockMovieTypeServices = new Mock<IMovieTypeServices>();
            _mockMovieRoomService = new Mock<IMovieRomeServices>();
            _mockSeatServices = new Mock<ISeatServices>();

            _controller = new cinemaRoomManager(
                _mockSeatServices.Object,
                _mockMovieRoomService.Object,
                _mockMovieTypeServices.Object,
                _mockMovieScheduleService.Object,
                _mockMovieService.Object,
                _mockScheduleService.Object,
                _mockScheduleSeatService.Object
            );
        }

        

        [Fact]
        public  void AddCinemaRoom_ShouldReturnCreated_WhenValidRoomIsProvided()
        {
            // Arrange
            var newRoom = new ResquestDTOMovieRoom { CinemeRoomName = "Room 3", SeatQuantity = 200 };

             _mockMovieRoomService.Setup(service => service.addMovieroom(newRoom))
                .Returns(Task.FromResult(true));

            // Act
            var result = _controller.AddCinemaRoom(newRoom);

            // Assert
            Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public void AddCinemaRoom_ShouldReturnBadRequest_WhenInvalidRoomIsProvided()
        {
            // Arrange
            var invalidRoom = new ResquestDTOMovieRoom { CinemeRoomName = "", SeatQuantity = 0 };

            _mockMovieRoomService.Setup(service => service.addMovieroom(invalidRoom))
                .Returns(Task.FromResult(false));

            // Act
            var result = _controller.AddCinemaRoom(invalidRoom);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void DeleteCinemaRoom_ShouldReturnNoContent_WhenRoomIsDeleted()
        {
            // Arrange
            int roomId = 1;

            _mockMovieRoomService.Setup(service => service.deleteMovieroom(roomId))
                .Returns(Task.FromResult(true));

            // Act
            var result = _controller.DeleteCinemaRoom(roomId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void DeleteCinemaRoom_ShouldReturnNotFound_WhenRoomDoesNotExist()
        {
            // Arrange
            int roomId = 99;

            _mockMovieRoomService.Setup(service => service.deleteMovieroom(roomId))
                .Returns(Task.FromResult(false));

            // Act
            var result = _controller.DeleteCinemaRoom(roomId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
