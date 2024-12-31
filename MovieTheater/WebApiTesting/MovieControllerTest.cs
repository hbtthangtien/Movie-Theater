using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers;
using WebAPI.Services;
using WebAPI.Services.DTO.Response;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Entity;
using WebAPI.Services.DTO;
using WebAPI.Services.DTO.Request;
using WebAPI.Constant;

namespace WebAPI.Tests
{
    public class MovieControllerTests
    {
        private readonly Mock<IMovieScheduleService> _movieScheduleServiceMock;
        private readonly Mock<IMovieService> _movieServiceMock;
        private readonly Mock<IScheduleService> _scheduleServiceMock;
        private readonly Mock<IScheduleSeatService> _scheduleSeatServiceMock;
        private readonly MovieController _movieController;

        public MovieControllerTests()
        {
            _movieScheduleServiceMock = new Mock<IMovieScheduleService>();
            _movieServiceMock = new Mock<IMovieService>();
            _scheduleServiceMock = new Mock<IScheduleService>();
            _scheduleSeatServiceMock = new Mock<IScheduleSeatService>();
            
        }

        [Fact]
        public void GetShowDateByMovieId_ReturnsOkResult()
        {
            // Arrange
            string movieId = "61020";
            var mockScheduleList = new List<MovieScheduleDTO> { new MovieScheduleDTO { ScheduleId = 1 } };
            var mockDate = new DateOnly(2023, 11, 15);
            _movieScheduleServiceMock.Setup(s => s.GetMovieScheduleByMovieID(movieId))
                .Returns(mockScheduleList);
            _scheduleServiceMock.Setup(s => s.GetDateByScheduleId(1)).Returns(mockDate);

            // Act
            var result = _movieController.GetShowDateByMovieId(movieId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var showDates = Assert.IsType<List<DateOnly?>>(okResult.Value);
            Assert.NotEmpty(showDates);
        }

        [Fact]
        public void GetShowsTimeByScheduleId_ReturnsOkResult()
        {
            // Arrange
            string scheduleId = "123";
            string date = "2023-11-15";
            DateOnly? scheduleDate = new DateOnly(2023, 11, 15);
            var mockTimeList = new List<ScheduleDTO> {new ScheduleDTO{ScheduleId=1, ScheduleTime = "1AM : 2AM",MovieScheduleDate = scheduleDate}};
            _scheduleServiceMock.Setup(s => s.GetTimeLineByScheduleId(scheduleId, date)).Returns(mockTimeList);

            // Act
            var result = _movieController.GetShowsTimeByScheduleId(scheduleId, date);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var times = Assert.IsType<List<string>>(okResult.Value);
            Assert.Contains("10:00", times);
        }

        [Fact]
        public void GetSeat_ReturnsOkResult()
        {
            // Arrange
            string movieId = "CinemaA1";
            int scheduleId = 1;
    
            // Tạo dữ liệu giả theo cấu trúc của ResponseDTOSeatRoom và ResponseDTOScheduleSeat
            var mockSeatList = new List<ResponseDTOSeatRoom>
            {
                new ResponseDTOSeatRoom
                {
                    CinemeRoomName = "CinemaA1",
                    ScheduleSeats = new List<ResponseDTOScheduleSeat>
                    {
                        new ResponseDTOScheduleSeat 
                        {
                            ScheduleSeatId = "05568", 
                            MovieId = movieId, 
                            ScheduleId = scheduleId, 
                            SeatId = 1, 
                            SeatColumn = "A", 
                            SeatRow = 1, 
                            SeatStatus = StatusSeat.SUCCESS, 
                            seatType_id = 1, 
                            CinemeRoomName = "CinemaA1",
                            price = 200.0
                        }
                    }
                }
            };
    
            _scheduleSeatServiceMock.Setup(s => s.GetListSeatRooms(movieId, scheduleId))
                .Returns(mockSeatList);

            // Act
            var result = _movieController.GetSeat(movieId, scheduleId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var seatRooms = Assert.IsType<List<ResponseDTOSeatRoom>>(okResult.Value);
            Assert.Single(seatRooms);
            Assert.Equal("CinemaA1", seatRooms[0].CinemeRoomName);
            Assert.Single(seatRooms[0].ScheduleSeats);
            Assert.Equal("1", seatRooms[0].ScheduleSeats[0].ScheduleSeatId);
        }


        [Fact]
        public void GetAllSeatsChoose_ReturnsOkResult()
        {
            // Arrange
            var scheduleSeats = new List<ResponseDTOScheduleSeat>
            {
                new ResponseDTOScheduleSeat
                {
                    ScheduleSeatId = "1",
                    MovieId = "M1",
                    ScheduleId = 1,
                    SeatId = 10,
                    SeatColumn = "A",
                    SeatRow = 1,
                    SeatStatus = StatusSeat.SUCCESS,
                    seatType_id = 2,
                    CinemeRoomName = "Room1",
                    price = 10.0
                }
            };

            var responseDTO = new ResponseDTOChooseSeat
            {
                movieName = "MovieName",
                cinemaRoom = "Room1",
                scheduleSeats = new List<RequestDTOScheduleSeat>
                {
                    new RequestDTOScheduleSeat
                    {
                        ScheduleSeatId = "1",
                        seatColumn = "A",
                        seatRow = 1,
                        seatType = new seatTypeDTO
                        {
                            Name = "Standard",
                            price = 10.0
                        }
                    }
                },
                scheduleShow = new DateOnly(2024, 11, 15),
                scheduleShowTime = "10:00 AM"
            };

            /*_scheduleSeatServiceMock.Setup(service => service.GetScheduleSeatsAsync(scheduleSeats))
                .Returns(Schedule);
*/
            // Act
            var result = _movieController.ConfirmChooseSeat(scheduleSeats);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ResponseDTOChooseSeat>(okResult.Value);
            Assert.Equal("MovieName", returnValue.movieName);
            Assert.Equal("Room1", returnValue.cinemaRoom);
            Assert.Single(returnValue.scheduleSeats);
            Assert.Equal("A", returnValue.scheduleSeats[0].seatColumn);
            Assert.Equal(10.0, returnValue.scheduleSeats[0].seatType.price);
        }

    }
}
