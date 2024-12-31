using WebAPI.Entity;
using WebAPI.Services.DTO;
using WebAPI.Services.DTO.Request;
using WebAPI.Services.DTO.Response;

namespace WebAPI.Services
{
    public interface IScheduleService
    {
        public List<ScheduleDTO> GetTimeLineByScheduleId(String id,string date);
        public DateOnly? GetDateByScheduleId(int id);
        public Task<bool> addSchedule(List<RequestDTOSchedule> schedule);
        public Task<bool> deleteSchedule(int scheduleid);

        public Task<(List<ResponseDTOSchedule> Schedules, int TotalCount, int TotalPages)> GetAllSchedulesPagedAsync(
            string? search, int page, int pageSize,string movieids);
        public Task<List<ResponseDTOSchedule>> GetAllSchedulesByMovieID(String movieid);
    }
}