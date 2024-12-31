using Microsoft.AspNetCore.Mvc;
using WebAPI.Entity;
using WebAPI.Services;
using WebAPI.Services.DTO.Request;

namespace WebAPI.Controllers;
[Route("api/manager/cinemarooms")]
[ApiController]
public class cinemaRoomManager:ControllerBase
{
    private readonly IMovieScheduleService _movieScheduleService;
    private readonly IMovieService _movieService;
    private readonly IScheduleService _scheduleService;
    private readonly IScheduleSeatService _scheduleSeatService;
    private readonly IMovieTypeServices _movieTypeServices;
    private readonly IMovieRomeServices _movieroomService;
    public readonly ISeatServices _seatServices;
    public cinemaRoomManager(ISeatServices seatServices,IMovieRomeServices movieroomService,IMovieTypeServices movieTypeServices,IMovieScheduleService movieScheduleService, IMovieService movieService,IScheduleService scheduleService,IScheduleSeatService scheduleSeatService)
    {
        _movieService = movieService;
        _movieScheduleService = movieScheduleService;
        _scheduleService = scheduleService;
        _scheduleSeatService = scheduleSeatService;
        _movieTypeServices = movieTypeServices;
        _movieroomService = movieroomService;
        _seatServices = seatServices;
    }
    
    [HttpDelete("cinemaroom")]
    public async Task<IActionResult> DeleteCinemaRoom(int cinemaRoomId)
    {
        await _movieroomService.deleteMovieroom(cinemaRoomId);
        return Ok();
    }

    [HttpPut("cinemaroom")]
    public async Task<IActionResult> UpdateCinemaRoom(ResquestDTOMovieRoom room)
    {
        await _movieroomService.updateMovieroom(room);
        return Ok();
    }

    [HttpPost("cinemaroom")]
    public async Task<IActionResult> AddCinemaRoom(ResquestDTOMovieRoom room)
    {
        await _movieroomService.addMovieroom(room);
        return Ok();
    }

    [HttpDelete("seat")]
    public async Task<IActionResult> DeleteSeat(List<int> seatId)
    {
        await _seatServices.DeleteSeat(seatId);
        return Ok();
    }

    [HttpPost("seat")]
    public async Task<IActionResult> AddSeat(List<RequestDTOSeat> seatId)
    {
        await _seatServices.AddSeat(seatId);
        return Ok();
    }

    [HttpPut("seat")]
    public async Task<IActionResult> UpdateSeat(List<RequestDTOSeat> seatId)
    {
        await _seatServices.UpdateSeat(seatId);
        return Ok();
    }
    [HttpGet("listcinemaroom")]
    public async Task<IActionResult> GetListMovies()
    {
        var result = await _movieroomService.GetAllMovieRoom();
        return Ok(result);
    }

    [HttpGet("movieroom")]
    public async Task<IActionResult> GetMovieroom(int id)
    {
        var result = await _movieroomService.GetMovieRoomById(id);
        return Ok(result);
    }
    [HttpGet("paged")]
    public async Task<IActionResult> GetMovieRoomsPaged(string? search = "", int page = 1, int pageSize = 5)
    {
        try
        {
            var (movieRooms, totalCount, totalPages) = await _movieroomService.GetMovieRoomsPagedAsync(search, page, pageSize);

            var result = new
            {
                TotalCount = totalCount,
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = totalPages,
                MovieRooms = movieRooms
            };

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("listseatbycinemaid")]

    public async Task<IActionResult> GetSeatsByCinemaId(int cinemaId)
    {
        var seat = await _seatServices.GetAllSeatByCinemaID(cinemaId);
        return Ok(seat);
    }

}