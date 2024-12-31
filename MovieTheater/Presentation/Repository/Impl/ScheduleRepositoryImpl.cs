using System.Globalization;
using Microsoft.EntityFrameworkCore;
using WebAPI.Entity;

namespace WebAPI.Repository.Impl
{
    public class ScheduleRepositoryImpl : GenericRepositoryImpl<Schedule>, IScheduleRepository
    {
        public ScheduleRepositoryImpl(MovieTheaterContext context) : base(context)
        {
        }
        public List<Schedule> GetTimeLineByScheduleId(String id, string date)
        { 
            DateOnly dateconvert = DateOnly.ParseExact(date, "yyyy-MM-dd");
        var result= (from ms in _context.MovieSchedules
                join s in _context.Schedules on ms.ScheduleId equals s.ScheduleId
                where ms.MovieId == id && s.MovieScheduleDate == dateconvert
                    select s).ToList();
        return result;
        }

        public DateOnly? GetDateByScheduleId(int id)
        {
            DateOnly Today = DateOnly.FromDateTime(DateTime.Now);
            DateOnly? datelist= _context.Schedules.Where(s => s.ScheduleId == id && s.MovieScheduleDate >= Today)
                        .Select(s=>s.MovieScheduleDate).SingleOrDefault();
            return datelist;
        }

        public async Task<Schedule> AddSchedule(Schedule schedule,int roomid)
        {
            if (schedule.MovieScheduleDate == null || string.IsNullOrWhiteSpace(schedule.ScheduleTime))
            {
                throw new ArgumentException("Invalid schedule data.");
            }

            // Phân tích thời gian bắt đầu và kết thúc từ chuỗi ScheduleTime
            var times = schedule.ScheduleTime.Split(':');
            if (!DateTime.TryParseExact(times[0].Trim(), "h tt", null, DateTimeStyles.None, out DateTime newStartTime) ||
                !DateTime.TryParseExact(times[1].Trim(), "h tt", null, DateTimeStyles.None, out DateTime newEndTime))
            {
                throw new FormatException("ScheduleTime format is invalid. Expected format: '8 PM : 9 PM'.");
            }


            
            var existingSchedules = await _context.Schedules.Where(t=>t.MovieScheduleDate== schedule.MovieScheduleDate).ToListAsync();
            
            // Kiểm tra trùng thời gian
            if (existingSchedules.Count() != 0)
            {
                foreach (var existingSchedule in existingSchedules)
                {
                    var roomids = await _context.ScheduleSeats.Where(t => t.ScheduleId == existingSchedule.ScheduleId)
                        .ToListAsync();
                    
                    var existingTimes = existingSchedule.ScheduleTime?.Split(':');
                    if (existingTimes == null || existingTimes.Length != 2)
                        continue;
            
                    DateTime existingStartTime = DateTime.ParseExact(existingTimes[0].Trim(), "h tt", null);
                    DateTime existingEndTime = DateTime.ParseExact(existingTimes[1].Trim(), "h tt", null);

                    foreach (var room in roomids )
                    {
                        var check =await _context.Seats.Where(t=>t.SeatId==room.SeatId).FirstOrDefaultAsync();
                        if (newStartTime < existingEndTime && newEndTime > existingStartTime&& check.CinemaRoomId==roomid)
                        {
                            throw new InvalidOperationException("Schedule time conflicts with an existing schedule.");
                        }
                    }
                    
                }
            }

            // Thêm lịch chiếu mới vào cơ sở dữ liệu 
            await _context.Schedules.AddAsync(schedule);
            await _context.SaveChangesAsync();
            return schedule;
        }

        public async Task<bool> DeleteSchedule(int scheduleId)
        {
            
            await _context.Database.ExecuteSqlRawAsync(
                "DELETE FROM Movie_Schedule WHERE schedule_id = {0}", scheduleId);
            
            await _context.Database.ExecuteSqlRawAsync(
                "DELETE FROM Schedule_Seat WHERE schedule_id = {0}", scheduleId);
            
            await _context.Database.ExecuteSqlRawAsync(
                "DELETE FROM Schedule WHERE schedule_id = {0}", scheduleId);
            await _context.SaveChangesAsync();
            return true;
        }
        public IQueryable<Schedule> GetAllSchedulesQueryable()
        {
            return _context.Schedules.AsQueryable();
        }
    }
}
