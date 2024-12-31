using AutoMapper;
using System.Buffers;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using WebAPI.Entity;
using WebAPI.Repository;
using WebAPI.Services.DTO;
using WebAPI.Services.DTO.Request;
using WebAPI.Services.DTO.Response;

namespace WebAPI.Services.Impl
{
    public class ScheduleServiceImpl : GenericServices, IScheduleService
    {
        public ScheduleServiceImpl(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
        {
        }

        public List<ScheduleDTO> GetTimeLineByScheduleId(String id, string date)
        {
            var timeline = _context.Schedules.GetTimeLineByScheduleId(id, date);
            DateTime now = DateTime.Now;
            List<ScheduleDTO> ans = new List<ScheduleDTO>();
            foreach (var s in timeline)
            {
                var list = GetDate(s.ScheduleTime!);
                string pos = list[2];
                DateTime DateFrom = DateTime.Parse(date + " " + list[0] + ":00" + pos);
                DateTime DateTo = DateTime.Parse(date + " " + list[1] + ":00" + pos);

                if (now <= DateTo)
                {
                    var scheduleDto = new ScheduleDTO
                    {
                        MovieId = id,
                        MovieScheduleDate = s.MovieScheduleDate,
                        ScheduleId = s.ScheduleId,
                        ScheduleTime = s.ScheduleTime
                    };
                    ans.Add(scheduleDto);
                }
            }
            return ans;
        }

        public DateOnly? GetDateByScheduleId(int id)
        {
            return _context.Schedules.GetDateByScheduleId(id);
        }

        public async Task<bool> addMovieSchedule(RequestDTOSchedule schedule, Schedule add)
        {
            var movieschedule = new MovieSchedule();
            movieschedule.ScheduleId = add.ScheduleId;
            movieschedule.MovieId = schedule.movieID;
            await _context.MovieSchedules.AddAsync(movieschedule);
            await _context.CommitAsync();
            return true;
        }

        public async Task<bool> updateMovieRoom(RequestDTOSchedule schedule, Schedule add)
        {
            int scheduleId = add.ScheduleId;
            string movieId = schedule.movieID;
            int roomid = schedule.cinemaRoomID;
            await _context.ScheduleSeats.AddScheduleSeatAsync(scheduleId, movieId, roomid);
            return true;
        }
        public async Task<bool> addSchedule(List<RequestDTOSchedule> schedules)
        {
            foreach (var schedule in schedules)
            {
                var mappedSchedule = _mapper.Map<Schedule>(schedule);
                int roomid = schedule.cinemaRoomID;
                var add = await _context.Schedules.AddSchedule(mappedSchedule, roomid);
                var result = await addMovieSchedule(schedule, add);
                var schseat = await updateMovieRoom(schedule, add);
            }

            return true;
        }

        public async Task<bool> deleteSchedule(int scheduleid)
        {
            await _context.Schedules.DeleteSchedule(scheduleid);
            return true;
        }
        private List<string> GetDate(string date)
        {
            // 6 AM : 12 PM split lay (6,12)
            List<string> list = Regex.Matches(date, "\\d+")
                                .Cast<Match>().Select(m => m.Value)
                                .ToList();
            // lay hau to cua gio (AM or PM)
            List<string> list2 = Regex.Matches(date, "[a-zA-Z]+")
                                .Cast<Match>().Select(m => m.Value)
                                .ToList();
            list.Add(list2[1].ToUpper());
            return list;

        }
        public async Task<(List<ResponseDTOSchedule> Schedules, int TotalCount, int TotalPages)> GetAllSchedulesPagedAsync(string? search, int page, int pageSize, string movieids)
        {
            if (page < 1 || pageSize < 1)
                throw new ArgumentException("Page and pageSize must be greater than 0.");

            // Lấy danh sách lịch hoặc tìm kiếm theo tiêu chí
            var query = _context.MovieSchedules.GetAllMoviesScheduleQueryable();

            // Đếm tổng số lịch chiếu
            int totalCount = await query.CountAsync();

            // Tính tổng số trang
            int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            // Kiểm tra nếu số trang vượt quá giới hạn
            if (page > totalPages && totalCount > 0)
                throw new ArgumentException($"Page {page} exceeds the total number of pages {totalPages}.");

            // Phân trang
            var schedules = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Chuyển đổi sang DTO
            var scheduleDTOs = new List<ResponseDTOSchedule>();

            foreach (var sc in schedules)
            {
                var scheduleDTO = new ResponseDTOSchedule
                {
                    ScheduleId = sc.ScheduleId,
                    MovieId = sc.MovieId,
                };

                // Lấy thông tin movie
                var movie = await _context.Movies.GetByIdAsync(sc.MovieId);
                scheduleDTO.MovieNameEnglish = movie?.MovieNameEnglish;
                scheduleDTO.SmallImage = movie?.SmallImage;

                // Lấy thông tin ghế và phòng chiếu
                var scSeat = await _context.ScheduleSeats.GetAllAsync();
                var scSeatID = scSeat.Where(s => s.ScheduleId == sc.ScheduleId).Select(s => s.SeatId).FirstOrDefault();

                var seat = await _context.Seats.GetAllAsync();
                scheduleDTO.CinemaRoomId = seat.Where(t => t.SeatId == scSeatID).Select(t => t.CinemaRoomId).FirstOrDefault();

                var roomName = await _context.CinemaRooms.GetAllAsync();
                scheduleDTO.CinemeRoomName = roomName.Where(t => t.CinemaRoomId == scheduleDTO.CinemaRoomId)
                                                     .Select(i => i.CinemeRoomName)
                                                     .FirstOrDefault();

                // Lấy thời gian và ngày chiếu
                var schedulet = await _context.Schedules.GetAllAsync();
                scheduleDTO.ScheduleTime = schedulet.Where(s => s.ScheduleId == scheduleDTO.ScheduleId)
                                                    .Select(s => s.ScheduleTime)
                                                    .FirstOrDefault();

                scheduleDTO.MovieScheduleDate = schedulet.Where(s => s.ScheduleId == scheduleDTO.ScheduleId)
                                                         .Select(s => s.MovieScheduleDate)
                                                         .FirstOrDefault();

                // Thêm vào danh sách DTO
                scheduleDTOs.Add(scheduleDTO);
            }
            return (scheduleDTOs, totalCount, totalPages);
        }
        public async Task<List<ResponseDTOSchedule>> GetAllSchedulesByMovieID(string movieid)
        {
            var moviechList = await _context.MovieSchedules.GetAllAsync();
            var movies = moviechList.Where(t => t.MovieId == movieid).ToList();
            var checkdate = new List<ResponseDTOSchedule>();
            foreach (var movie in movies)
            {
                var datetime = DateTime.Now;
                var date = DateOnly.FromDateTime(datetime);
                var sch = await _context.Schedules.GetAllAsync();
                var cshdate = sch.Where(t => t.ScheduleId == movie.ScheduleId).FirstOrDefault();
                if (cshdate.MovieScheduleDate >= date)
                {
                    var sc = new ResponseDTOSchedule();
                    sc.ScheduleId = movie.ScheduleId;
                    sc.MovieId = movie.MovieId;
                    var movieed = await _context.Movies.GetByIdAsync(sc.MovieId);
                    sc.MovieNameEnglish = movieed?.MovieNameEnglish;
                    sc.SmallImage = movieed?.SmallImage;
                    var scSeat = await _context.ScheduleSeats.GetAllAsync();
                    var scSeatID = scSeat.Where(s => s.ScheduleId == sc.ScheduleId).Select(s => s.SeatId).FirstOrDefault();

                    var seat = await _context.Seats.GetAllAsync();
                    sc.CinemaRoomId = seat.Where(t => t.SeatId == scSeatID).Select(t => t.CinemaRoomId).FirstOrDefault();

                    var roomName = await _context.CinemaRooms.GetAllAsync();
                    sc.CinemeRoomName = roomName.Where(t => t.CinemaRoomId == sc.CinemaRoomId)
                        .Select(i => i.CinemeRoomName)
                        .FirstOrDefault();
                    sc.ScheduleTime = cshdate.ScheduleTime;
                    sc.MovieScheduleDate = cshdate.MovieScheduleDate;
                    checkdate.Add(sc);
                }
            }
            return checkdate;
        }
    }
}
