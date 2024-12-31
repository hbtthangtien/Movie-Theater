using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebAPI.Hubs;
using WebAPI.Services;
using WebAPI.Services.DTO.Request;
using WebAPI.Services.DTO.Response;
namespace WebAPI.Controllers
{
    [Route("api/movies")]
    [ApiController]
    
    public class MovieController: ControllerBase
    {
        private readonly IMovieScheduleService _movieScheduleService;
        private readonly IMovieService _movieService;
        private readonly IScheduleService _scheduleService;
        private readonly IScheduleSeatService _scheduleSeatService;
        private readonly IMovieTypeServices _movieTypeServices;
        private readonly IHubContext<SeatStatusHub> _hubContext;
        public MovieController(IMovieTypeServices movieTypeServices,IMovieScheduleService movieScheduleService,
            IMovieService movieService,IScheduleService scheduleService,IScheduleSeatService scheduleSeatService
            , IHubContext<SeatStatusHub> hub)
        {
            _movieService = movieService;
            _movieScheduleService = movieScheduleService;
            _scheduleService = scheduleService;
            _scheduleSeatService = scheduleSeatService;
            _movieTypeServices = movieTypeServices;
            _hubContext = hub;
        }
        [HttpGet("showsday/{id}")]
        public IActionResult GetShowDateByMovieId(String id)
        {
            
            var showdate = _movieScheduleService.GetMovieScheduleByMovieID(id);
            List<DateOnly?> showdates = new List<DateOnly?>();
            int count =showdate.Count();
            for (int i = 0; i < count; i++)
            {
                int? show = showdate[i].ScheduleId;
                if (show.HasValue)
                {
                    var date = _scheduleService.GetDateByScheduleId(show.Value);
                    if (date.HasValue)
                    {
                        showdates.Add(date);
                    }
                    
                }                
            }
            var result = showdates.Distinct().ToList();
            result.Sort();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovieByShowId(String id)
        {
            var movie = await _movieService.GetMovieById(id);
            return Ok(movie);
        }
        [HttpGet("showstime/{id}/{date}")]
        public IActionResult GetShowsTimeByScheduleId(String id,string date)
        {
            var time = _scheduleService.GetTimeLineByScheduleId(id,date);
            return Ok(time);
            
        }

        [HttpGet("seats/{MovieID}/{SchID}")]
        public IActionResult GetSeat(String MovieID,int SchID)
        {
            var seat = _scheduleSeatService.GetListSeatRooms(MovieID,SchID);
            return Ok(seat);
        }

        [HttpPost("seats")]
        public async Task<IActionResult> ConfirmChooseSeat(List<ResponseDTOScheduleSeat> scheduleSeats)
        {
            try
            {
                var username = (HttpContext.User?.Identity?.Name) ?? throw new System.Exception("User is not authenticated!!!");
                var bookingSeat =await _scheduleSeatService.ExecuteSelectedSeat(scheduleSeats,username);
                await _hubContext.Clients.All.SendAsync("updateSeat", bookingSeat.scheduleSeats);
                return Ok(bookingSeat);
            }catch(System.Exception e)
            {
                return BadRequest(new ResponseDTOApi
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    StatusMessage = e.Message
                });
            }
        }
        [HttpGet("ScheduleSeats/{MovieId}/{ScheduleId}")]
        public async Task<IActionResult> GetAllSelectSeat(string MovieId, int ScheduleId)
        {
            try
            {
                var username = (HttpContext.User?.Identity?.Name) ?? throw new System.Exception("User is not authenticated!!!");
                var request = new RequestDTOSeats 
                { 
                    MovieId = MovieId,
                    ScheduleId = ScheduleId,
                    username = username
                };
                var result =await _scheduleSeatService.GetAllSeatsSelected(request);
                return Ok(result);
            }
            catch(System.Exception e) {
                return BadRequest(new ResponseDTOApi
                {
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    StatusMessage = e.Message
                });
            }
        }

    }
}