using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using WebAPI.Constant;
using WebAPI.Entity;
using WebAPI.Services;
using WebAPI.Services.DTO.Response;

namespace WebAPI.Hubs
{
    public class SeatStatusHub : Hub
    {
        private readonly IScheduleSeatService _scheduleSeatService;
        private readonly IServiceProvider _serviceProvider;
        public SeatStatusHub(IScheduleSeatService scheduleSeat, IServiceProvider serviceProvider)
        {
            _scheduleSeatService = scheduleSeat;
            _serviceProvider = serviceProvider;
        }
       
        public async Task ReserveUntil(List<string> request)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<MovieTheaterContext>();
                var seats = await dbContext.ScheduleSeats.Where(e => request.Contains(e.ScheduleSeatId)).ToListAsync();
                foreach(var i in seats)
                {
                    i.SeatStatus = StatusSeat.EMPTY;
                    i.AccountId = null;
                    i.ReservedUntil = null;
                }
                await dbContext.SaveChangesAsync();
                await Clients.All.SendAsync("reserveSeat", request);
            }                            
        }
    }
}
