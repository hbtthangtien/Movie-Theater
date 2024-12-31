using Microsoft.EntityFrameworkCore;
using WebAPI.Entity;

namespace WebAPI.Repository.Impl
{
    public class SeatRepositoryImpl : GenericRepositoryImpl<Seat>, ISeatRepository
    {
        public SeatRepositoryImpl(MovieTheaterContext context) : base(context)
        {
        }

        public async Task<bool> AddSeat(List<Seat> seat)
        {
            foreach (var seats in seat)
            {
                var upSeat = new Seat();
                upSeat.SeatId = 0;
                upSeat.CinemaRoomId = seats.CinemaRoomId;
                upSeat.SeatColunm=seats.SeatColunm;
                upSeat.SeatRow = seats.SeatRow;
                upSeat.SeatStatus = seats.SeatStatus;
                upSeat.seatType_id = seats.seatType_id;
                await _context.Seats.AddAsync(upSeat); 
            }
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateSeat(List<Seat> seatList)
        {
            foreach (var seat in seatList)
            {
                
                var existingSeat = await _context.Seats.FindAsync(seat.SeatId);
                if (existingSeat != null)
                {
                    
                    existingSeat.CinemaRoomId = seat.CinemaRoomId;
                    existingSeat.SeatColunm = seat.SeatColunm;
                    existingSeat.SeatRow = seat.SeatRow;
                    existingSeat.SeatStatus = seat.SeatStatus;
                    existingSeat.seatType_id = seat.seatType_id;
                    await _context.Database.ExecuteSqlRawAsync(
                        "UPDATE Schedule_Seat " +
                        "SET seat_column = {0}, seat_row = {1}, seat_status = {2}, seatType_id = {3} " +
                        "WHERE seat_id = {4}",
                        seat.SeatColunm, seat.SeatRow, seat.SeatStatus, seat.seatType_id, seat.SeatId);

                    
                    _context.Seats.Update(existingSeat);
                }
                else
                {
                    //log here
                }
            }

            
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteSeat(List<int> seatIds)
        {
            foreach (var seatId in seatIds)
            {
               
                var seat = await _context.Seats.Where(t=>t.SeatId==seatId).FirstOrDefaultAsync();
                
                if (seat != null)
                {
                    await _context.Database.ExecuteSqlRawAsync(
                        "DELETE FROM Schedule_Seat WHERE seat_id = {0}", seatId);
                    _context.Seats.Remove(seat);
                }
                else
                {
                    // Tùy chọn: Log thông báo hoặc bỏ qua nếu ghế không tồn tại
                    // Ví dụ: throw new Exception($"Seat with ID {seatId} not found.");
                }
            }

            
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Seat>> GetAllSeatByCinemaID(int cinemaId)
        {
            var seat= await _context.Seats.Where(t => t.CinemaRoomId == cinemaId).ToListAsync();
            return seat;
        }
    }
}
