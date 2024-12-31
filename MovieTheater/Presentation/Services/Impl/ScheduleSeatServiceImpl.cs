using AutoMapper;
using WebAPI.Repository;
using WebAPI.Services.DTO.Request;
using WebAPI.Services.DTO.Response;
using System.Linq;
using Azure.Core;
using WebAPI.Entity;
using WebAPI.Services.DTO;
using WebAPI.Exception;
using WebAPI.Constant;
using Microsoft.IdentityModel.Tokens;

namespace WebAPI.Services.Impl
{
    public class ScheduleSeatServiceImpl : GenericServices, IScheduleSeatService
    {
        public ScheduleSeatServiceImpl(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
        {
        }

        public List<ResponseDTOScheduleSeat> GetScheduleSeats(string movieId, int schid)
        {
            var seat = _context.ScheduleSeats.GetScheduleSeatsBymovieidAndschid(movieId, schid);
            var relst = _mapper.Map<List<ResponseDTOScheduleSeat>>(seat);
            int count = relst.Count;
            for (int i = 0; i < count; i++)
            {
                relst[i].CinemeRoomName = _context.ScheduleSeats.GetMovieCinemaNameBySeatId(relst[i].SeatId);
                relst[i].price = _context.ScheduleSeats.GetPriceBySeatTypeId(relst[i].seatType_id);
            }
            return relst;
        }
        public static List<string> RemoveDuplicates(List<string> inputList)
        {
            return inputList.Distinct().ToList();
        }
        public List<ResponseDTOSeatRoom> GetListSeatRooms(string movieId, int schid)
        {
            List<ResponseDTOScheduleSeat> listSeatRaw = GetScheduleSeats(movieId, schid);
            List<string> nameRoomRaw = new List<string>();
            List<ResponseDTOSeatRoom> lRoom = new List<ResponseDTOSeatRoom>();

            // Lấy danh sách tên phòng từ các ghế
            int count = listSeatRaw.Count;
            for (int i = 0; i < count; i++)
            {
                nameRoomRaw.Add(listSeatRaw[i].CinemeRoomName);
            }

            // Loại bỏ trùng lặp trong danh sách tên phòng
            List<string> nameRoom = RemoveDuplicates(nameRoomRaw);
            int countRoom = nameRoom.Count;

            // Tạo các đối tượng ResponseDTOSeatRoom trong danh sách lRoom
            for (int i = 0; i < countRoom; i++)
            {
                // Khởi tạo đối tượng mới và đặt tên phòng
                var room = new ResponseDTOSeatRoom
                {
                    CinemeRoomName = nameRoom[i],
                    ScheduleSeats = new List<ResponseDTOScheduleSeat>()  // Khởi tạo danh sách ghế trống
                };

                // Thêm các ghế vào phòng phù hợp
                for (int j = 0; j < count; j++)
                {
                    if (listSeatRaw[j].CinemeRoomName.Contains(nameRoom[i]))
                    {
                        room.ScheduleSeats.Add(listSeatRaw[j]);
                    }
                }

                // Thêm phòng vào danh sách lRoom
                lRoom.Add(room);
            }

            return lRoom;
        }

        public async Task<List<RequestDTOScheduleSeat>> ConvertScheduleSeat(List<ResponseDTOScheduleSeat> scheduleSeats)
        {
            List<RequestDTOScheduleSeat> seats = new List<RequestDTOScheduleSeat>();
            var count = scheduleSeats.Count;
            for (int i = 0; i < count; i++)
            {
                RequestDTOScheduleSeat seat = new RequestDTOScheduleSeat();
                seat.ScheduleSeatId = scheduleSeats[i].ScheduleSeatId;
                seat.seatColumn = scheduleSeats[i].SeatColumn;
                seat.seatRow = scheduleSeats[i].SeatRow;
                seatTypeDTO sTD = new seatTypeDTO();
                sTD.price = scheduleSeats[i].price;
                sTD.Name = _context.ScheduleSeats.GetNameBySeatTypeId(scheduleSeats[i].seatType_id);
                seat.seatType = sTD;
                seat.ReverseUntil = await _context.ScheduleSeats.GetReserveUntilById(scheduleSeats[i].ScheduleSeatId);
                seat.seatStatus = await _context.ScheduleSeats.GetStatusSeatById(scheduleSeats[i].ScheduleSeatId);
                seats.Add(seat);

            }
            return seats;
        }
        public async Task<ResponseDTOChooseSeat> GetScheduleSeatsAsync(List<ResponseDTOScheduleSeat> scheduleSeats, string AccountId)
        {
            var seats = await ConvertScheduleSeat(scheduleSeats);
            ResponseDTOChooseSeat seat = new ResponseDTOChooseSeat();
            seat.scheduleSeats = seats;
            seat.cinemaRoom = scheduleSeats[0].CinemeRoomName;
            seat.movieName = _context.ScheduleSeats.GetMovieNameVNByMovieId(scheduleSeats[0].MovieId);
            var time = _context.ScheduleSeats.GetScheduleById(scheduleSeats[0].ScheduleId);
            seat.scheduleShow = (DateOnly)time.MovieScheduleDate;
            seat.scheduleShowTime = time.ScheduleTime;
            seat.AccountId = AccountId;
            return seat;
        }

        public async Task ReserveSeat(string ScheduleSeatId)
        {
            var entity = await _context.ScheduleSeats.GetByIdAsync(ScheduleSeatId)
                ?? throw new NotFoundException($"This ScheduleSeat with Id: {ScheduleSeatId} is not exist");
            entity.SeatStatus = StatusSeat.EMPTY;
            entity.ReservedUntil = null;
            entity.AccountId = null;
            _context.ScheduleSeats.Update(entity);
            await _context.CommitAsync();
        }

        public async Task HoldSeat(string ScheduleSeatId, string AccountId)
        {

            var entity = await _context.ScheduleSeats.GetByIdAsync(ScheduleSeatId)
                ?? throw new NotFoundException($"This ScheduleSeat with Id: {ScheduleSeatId} is not exist");
            entity.SeatStatus = StatusSeat.HOLD;
            entity.AccountId = AccountId;
            entity.ReservedUntil = DateTime.Now.AddMinutes(1);
            _context.ScheduleSeats.Update(entity);
            await _context.CommitAsync();
        }

        public async Task<ResponseDTOChooseSeat> ExecuteSelectedSeat(List<ResponseDTOScheduleSeat> request, string username)
        {
            var user = await _context.Accounts.FindByNameAsync(username);
            string errors = "";
            foreach (var seat in request)
            {
                string check = await _context.ScheduleSeats.GetStatusSeatById(seat.ScheduleSeatId);
                if (!check.Equals(StatusSeat.EMPTY)) errors += $"{seat.SeatColumn + seat.SeatRow}, ";
            }
            if (!errors.IsNullOrEmpty()) 
            {
                errors = errors.Remove(errors.Length - 2);
                errors = "These Schedule Seats "+ errors+" is not empty";             
                    throw new System.Exception(errors);
            }
            foreach (var seat in request)
            {
                string check = await _context.ScheduleSeats.GetStatusSeatById(seat.ScheduleSeatId);
                if (!check.Equals(StatusSeat.EMPTY)) throw new System.Exception($"This Schedule Seat {seat.SeatColumn + seat.SeatRow} is not empty");
                await HoldSeat(seat.ScheduleSeatId, user.Id!);
            }
            var response = await GetScheduleSeatsAsync(request, user.Id);
            return response;
        }

        public async Task SelectedSeat(string ScheduleSeatId)
        {
            var entity = await _context.ScheduleSeats.GetByIdAsync(ScheduleSeatId)
                ?? throw new NotFoundException($"This ScheduleSeat with Id: {ScheduleSeatId} is not exist");
            entity.SeatStatus = StatusSeat.SUCCESS;
            entity.ReservedUntil = null;
            _context.ScheduleSeats.Update(entity);
            await _context.CommitAsync();
        }

        public async Task ReserveSelectedSeat(List<RequestDTOScheduleSeat> requests)
        {
            foreach (var seat in requests)
            {
                string check = await _context.ScheduleSeats.GetStatusSeatById(seat.ScheduleSeatId);
                if (!check.Equals(StatusSeat.HOLD))
                    throw new System.Exception($"Can't not cancel Schedule Seat {seat.seatColumn + seat.seatRow} because it's {check.ToLower()}");
                await ReserveSeat(seat.ScheduleSeatId);
            }
        }

        public async Task<List<ResponseDTOSeats>> GetAllSeatsSelected(RequestDTOSeats request)
        {
            var user = await _context.Accounts.FindByNameAsync(request.username);
            var result = await _context.ScheduleSeats.GetAllSeatsSelected(request, user.Id);
            return _mapper.Map<List<ResponseDTOSeats>>(result);

        }
    }
}