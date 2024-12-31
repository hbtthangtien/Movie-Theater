using WebAPI.Entity;

namespace WebAPI.Repository
{
    public interface IScheduleRepository : IGenericRepository<Schedule>
    {
        public List<Schedule> GetTimeLineByScheduleId(String id, string date);
        public DateOnly? GetDateByScheduleId(int id);
        public Task<Schedule> AddSchedule(Schedule schedule,int roomid);
        public Task<bool> DeleteSchedule(int scheduleId);
        public IQueryable<Schedule> GetAllSchedulesQueryable();
    }
}

