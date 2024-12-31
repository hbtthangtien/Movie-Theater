using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Services;
using WebAPI.Services.DTO;
using WebAPI.Services.DTO.Response;

namespace WebAPI.Controllers;
[Route("api/manager/movies")]
[ApiController]
public class ManagerMovie :ControllerBase
{
    private readonly IMovieScheduleService _movieScheduleService;
    private readonly IMovieService _movieService;
    private readonly IScheduleService _scheduleService;
    private readonly IScheduleSeatService _scheduleSeatService;
    private readonly IMovieTypeServices _movieTypeServices;
    private readonly IMovieRomeServices _movieroomService;
    public ManagerMovie(IMovieRomeServices movieroomService,IMovieTypeServices movieTypeServices,
        IMovieScheduleService movieScheduleService, IMovieService movieService,
        IScheduleService scheduleService,IScheduleSeatService scheduleSeatService)
    {
        _movieService = movieService;
        _movieScheduleService = movieScheduleService;
        _scheduleService = scheduleService;
        _scheduleSeatService = scheduleSeatService;
        _movieTypeServices = movieTypeServices;
        _movieroomService= movieroomService;
    }
    //admin movie controller    
    [HttpGet("allmovies")]
    public async Task<IActionResult> GetAllMovies()
    {
        var movies = await _movieService.GetAllMovies();
        return Ok(movies);
    }
    [HttpGet("movie/{id}")]
    public async Task<IActionResult> GetMovieByShowId(String id)
    {
        var movie = await _movieService.GetMovieById(id);
        return Ok(movie);
    }
    [HttpGet("movietype")]
    public async Task<IActionResult> GetMovieTypes()
    {
        var result= _movieTypeServices.GetAllMovieTypes();
        return Ok(result);
    }
    [HttpPut("updatemovie")]
    public async Task<IActionResult> UpdateMovie([FromForm] MovieDTO movie, IFormFile? largeImage, IFormFile? smallImage)
    {
        try
        {
            var result = await _movieService.UpdateMovie(movie, largeImage, smallImage);

            if (result)
            {
                return Ok("Movie updated successfully.");
            }

            return BadRequest("Validation failed or other errors occurred.");
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { Errors = ex.Message });
        }
    }

    [HttpDelete("deletemovie")]
    public async Task<IActionResult> DeleteMovie(String id)
    {
        var movie = await _movieService.DeleteMovieAsync(id);
        return Ok(movie);
    }

    [HttpPost("moviecreate")]
    public async Task<IActionResult> AddMovie([FromForm] ResponseDTOMovie movie, IFormFile? largeImage, IFormFile? smallImage)
    {
        var (success, errors) = await _movieService.AddMovieAsync(movie, largeImage, smallImage);

        if (!success)
        {
            return BadRequest(new { Errors = errors });
        }

        return Ok("Movie added successfully.");
    }
        
    
    [HttpGet("paged")]
    public async Task<IActionResult> GetMoviesPaged(int status, string? search = "", int page = 1, int pageSize = 5)
    {
        try
        {
            // Gọi dịch vụ để lấy danh sách phim phân trang theo status
            var (movies, totalCount, totalPages) = await _movieService.GetMoviesPagedAsync(search, page, pageSize, status);

            // Định dạng kết quả trả về
            var result = new
            {
                TotalCount = totalCount, // Tổng số lượng phim
                CurrentPage = page,     // Trang hiện tại
                PageSize = pageSize,    // Kích thước mỗi trang
                TotalPages = totalPages,// Tổng số trang
                Movies = movies         // Danh sách phim trong trang
            };

            // Trả về kết quả thành công
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            // Xử lý lỗi tham số không hợp lệ
            return BadRequest(new
            {
                Error = ex.Message,
                Status = status,
                Search = search,
                Page = page,
                PageSize = pageSize
            });
        }
        catch (System.Exception ex)
        {
            // Xử lý lỗi chung
            return StatusCode(500, new
            {
                Error = "An unexpected error occurred.",
                Details = ex.Message
            });
        }
    }

    

}