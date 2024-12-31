using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using WebAPI.Constant;
using WebAPI.Entity;
using WebAPI.Hubs;
namespace WebAPI.BackgroundServices

{
    public class SeatStatusBackgroundService : BackgroundService
    {

        private readonly IServiceProvider _serviceProvider;
        private readonly IHubContext<SeatStatusHub> _hubContext;

        public SeatStatusBackgroundService(IServiceProvider serviceProvider, IHubContext<SeatStatusHub> hub)
        {
            _serviceProvider = serviceProvider;
            _hubContext = hub;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<MovieTheaterContext>();
                    var expiredSeats = dbContext.ScheduleSeats
                        .Where(s => s.SeatStatus.Equals(StatusSeat.HOLD) && s.ReservedUntil <= DateTime.Now)
                        .ToList();

                    if (expiredSeats.Any())
                    {
                        foreach (var seat in expiredSeats)
                        {
                            seat.SeatStatus = StatusSeat.EMPTY;
                            seat.ReservedUntil = null;
                            seat.AccountId = null;
                        }
                        var listId = expiredSeats.Select(e => e.ScheduleSeatId).ToList();
                        await dbContext.SaveChangesAsync();
                        await _hubContext.Clients.All.SendAsync("reserveSeat", listId);
                    }
                } 
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Background Service is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}
