using Microsoft.EntityFrameworkCore;
using WebAPI.Constant;
using WebAPI.Entity;
using WebAPI.Exception;
using WebAPI.Services.DTO.Request;

namespace WebAPI.Repository.Impl
{
    public class ScheduleSeatRepositoryImpl : GenericRepositoryImpl<ScheduleSeat>, IScheduleSeatRepository
    {
        public ScheduleSeatRepositoryImpl(MovieTheaterContext context) : base(context)
        {
        }

        public List<ScheduleSeat> GetScheduleSeatsBymovieidAndschid(string movieId, int schId)
        {
            var seat= _context.ScheduleSeats.Where(s=> s.MovieId==movieId && s.ScheduleId==schId).ToList();
            return seat;
        }

        public string? GetMovieCinemaNameBySeatId(int? seatId)
        {
            String? name=(from s in _context.ScheduleSeats where s.SeatId== seatId
                join st in _context.Seats on s.SeatId equals st.SeatId
                join cm in _context.CinemaRooms on st.CinemaRoomId equals cm.CinemaRoomId
                select cm.CinemeRoomName).FirstOrDefault();
            return name;
        }

        public double GetPriceBySeatTypeId(int? seatTypeId)
        {
            double price= (double)(from s in _context.ScheduleSeats where s.seatType_id == seatTypeId
                    join ts in _context.TypeSeats on s.seatType_id equals ts.Id
                    select ts.price).FirstOrDefault()!;
            return price;
        }

        public string? GetNameBySeatTypeId(int? seatTypeId)
        {
            var name=(from s in _context.ScheduleSeats where s.seatType_id == seatTypeId
                join ts in _context.TypeSeats on s.seatType_id equals ts.Id
                select ts.Name).FirstOrDefault();
            return name;
        }

        public string? GetMovieNameVNByMovieId(string? Id)
        {
            var name = _context.Movies.Where(t=>t.MovieId==Id).FirstOrDefault();
            return name.MovieNameEnglish;
        }

        public string? GetMovieNameENGByMovieId(string? Id)
        {
            var name = _context.Movies.Where(t=>t.MovieId==Id).FirstOrDefault();
            return name.MovieNameEnglish;
        }

        public Schedule GetScheduleById(int? id)
        {
            var sCh=_context.Schedules.Where(t=>t.ScheduleId == id).FirstOrDefault();
            return sCh;
        }

        public ScheduleSeat GetScheduleSeatById(string? id)
        {
            var ss=_context.ScheduleSeats.Where(t=>t.ScheduleSeatId == id).FirstOrDefault();
            return ss;
        }

        public async Task<string> GetStatusSeatById(string? ScheduleSeatId)
        {
            var ScheduleSeat = await _context.FindAsync<ScheduleSeat>(ScheduleSeatId)
                ?? throw new NotFoundException($"This ScheduleSeat with Id: {ScheduleSeatId} is not exist");
            return ScheduleSeat.SeatStatus!;
        }

        public async Task<DateTime> GetReserveUntilById(string? ScheduleSeatId)
        {
            var ScheduleSeat = await _context.FindAsync<ScheduleSeat>(ScheduleSeatId)
                ?? throw new NotFoundException($"This ScheduleSeat with Id: {ScheduleSeatId} is not exist");
            return (DateTime)ScheduleSeat.ReservedUntil!;
        }
        public async Task<bool> AddScheduleSeatAsync(int scheduleId, string movieId, int roomid)
        {
            
            var seats = await _context.Seats.Where(t => t.CinemaRoomId == roomid).ToListAsync();
            var currentDateTime = DateTime.Now.ToString("mmss");
            foreach (var seat in seats)
            {
                var scheduleseat = new ScheduleSeat();
                scheduleseat.ScheduleSeatId = $"{currentDateTime}{seat.SeatId}";
                scheduleseat.ScheduleId = scheduleId;
                scheduleseat.MovieId = movieId;
                scheduleseat.SeatId = seat.SeatId;
                scheduleseat.SeatColumn=seat.SeatColunm;
                scheduleseat.SeatRow=seat.SeatRow;
                scheduleseat.SeatStatus="EMPTY";
                scheduleseat.seatType_id=seat.seatType_id;
                await _context.ScheduleSeats.AddAsync(scheduleseat);
            }
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ScheduleSeat>> GetAllSeatsSelected(RequestDTOSeats request,string AccountId)
        {
            
            var result =await _context.ScheduleSeats
                .Include(e=> e.TypeSeat)
                .Where(e => e.MovieId == request.MovieId &&
                e.ScheduleId == request.ScheduleId &&
                e.AccountId == AccountId && e.SeatStatus == StatusSeat.HOLD).ToListAsync() ?? throw new NotFoundException($"There are no selected by userId {AccountId}");
            return result;
        }
    }
}
