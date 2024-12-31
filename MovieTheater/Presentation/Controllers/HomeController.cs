using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Constant;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Microsoft.AspNetCore.Components.Route("api/home")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public HomeController(IMovieService movieService)
        {
            _movieService = movieService;
        }



        [HttpGet("movies/{name}")]
        public IActionResult GetMoviesByName(String name)
        {
            var movie = _movieService.GetMovieByName(name);
            return Ok(movie);
        }

        [HttpGet("movies/incoming")]
        public async Task<IActionResult> GetMoviesIncoming()
        {
            var movie = await _movieService.GetMovieInComing();
            return Ok(movie);
        }

        [HttpGet("movies/upcoming")]
        public async Task<IActionResult> GetMoviesUpcoming()
        {
            var movie = await _movieService.GetMovieUpComing();
            return Ok(movie);
        }
    }
}