using Microsoft.AspNetCore.Mvc;
using WebAPI.Entity;
using WebAPI.Services;
using WebAPI.Services.DTO.Request;

namespace WebAPI.Controllers;
[Route("api/manager/schdule")]
[ApiController]
public class ManagerSchdule : ControllerBase
{
    private readonly IMovieScheduleService _movieScheduleService;
    private readonly IMovieService _movieService;
    private readonly IScheduleService _scheduleService;
    private readonly IScheduleSeatService _scheduleSeatService;
    private readonly IMovieTypeServices _movieTypeServices;
    private readonly IMovieRomeServices _movieroomService;
    public ManagerSchdule(IMovieRomeServices movieroomService,IMovieTypeServices movieTypeServices,IMovieScheduleService movieScheduleService, IMovieService movieService,IScheduleService scheduleService,IScheduleSeatService scheduleSeatService)
    {
        _movieService = movieService;
        _movieScheduleService = movieScheduleService;
        _scheduleService = scheduleService;
        _scheduleSeatService = scheduleSeatService;
        _movieTypeServices = movieTypeServices;
        _movieroomService = movieroomService;
    }

    [HttpPost("moviesettime")]
    public async Task<IActionResult> AddMoviesettime(List<RequestDTOSchedule> movieSchedule)
    {
        var sch = await _scheduleService.addSchedule(movieSchedule);
        return Ok(sch);
    }

    [HttpDelete("scheduleremove")]
    public async Task<IActionResult> RemoveSchedule(int scheduleid)
    {
        var check= await _scheduleService.deleteSchedule(scheduleid);
        return Ok(check);
    }

    [HttpGet("allschedule")]
    public async Task<IActionResult> GetAllSchedule(string movieid,int pageSize = 5,  int page = 1)
    {
        var result = await _scheduleService.GetAllSchedulesPagedAsync(null,page,pageSize,movieid);
        return Ok(new
        {
            result.Schedules,
            result.TotalCount,
            result.TotalPages,
            CurrentPage = page
        });
    }

    [HttpGet("schedulebymovieid")]
    public async Task<IActionResult> GetScheduleByMovieId(string movieid)
    {
        var result = await _scheduleService.GetAllSchedulesByMovieID(movieid);
        return Ok(result);
    }

    [HttpGet("movieincoming")]
    public async Task<IActionResult> GetMoviesPaged(string? search = "", int page = 1, int pageSize = 5)
    {
        try
        {
            var (movies, totalCount, totalPages) = await _movieService.GetMoviesincomingAsync(search, page, pageSize);

            var result = new
            {
                TotalCount = totalCount,
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = totalPages,
                Movies = movies
            };

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}