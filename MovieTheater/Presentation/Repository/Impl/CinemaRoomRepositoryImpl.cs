using Microsoft.EntityFrameworkCore;
using WebAPI.Entity;

namespace WebAPI.Repository.Impl
{
    public class CinemaRoomRepositoryImpl : GenericRepositoryImpl<CinemaRoom>, ICinemaRoomRepository
    {
        public CinemaRoomRepositoryImpl(MovieTheaterContext context) : base(context)
        {
        }

        public async Task<CinemaRoom> AddCinemaRoom(CinemaRoom cinemaRoom)
        {
            var room = new CinemaRoom();
            room.CinemeRoomName = cinemaRoom.CinemeRoomName;
            room.SeatQuantity = cinemaRoom.SeatQuantity;
            room.CinemaRoomId = 0;
            var re= await _context.CinemaRooms.AddAsync(room);
            await _context.SaveChangesAsync();
            return re.Entity;
        }

        public async Task<bool> updateinemaRoom(CinemaRoom cinemaRoom)
        {
            var room = await _context.CinemaRooms.FirstOrDefaultAsync(m=>m.CinemaRoomId == cinemaRoom.CinemaRoomId);
            room.CinemeRoomName = cinemaRoom.CinemeRoomName;
            room.SeatQuantity = cinemaRoom.SeatQuantity;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> deleteMovieroom(int movieroom)
        {
            var room= await _context.CinemaRooms.FirstOrDefaultAsync(m => m.CinemaRoomId == movieroom);
            var seats=await _context.Seats.Where(t=> t.CinemaRoomId == movieroom).ToListAsync();
            foreach (var seat in seats)
            {
                _context.Seats.Remove(seat);
            }
           
             _context.CinemaRooms.Remove(room);
            await _context.SaveChangesAsync();
            return true;
        }
        public IQueryable<CinemaRoom> GetAllQueryable()
        {
            return _context.CinemaRooms.AsQueryable();
        }

        public IQueryable<CinemaRoom> GetMovieRoomsByName(string name)
        {
            return _context.CinemaRooms.Where(r => r.CinemeRoomName.Contains(name)).AsQueryable();
        }

        public async Task<CinemaRoom> GetMovieRoomByID(int id)
        {
            var movie = await _context.CinemaRooms.FirstOrDefaultAsync(m => m.CinemaRoomId == id);
            return movie;
        }

        public async Task<bool> autoAddSeat(int cinemaID, int? seatQuantity)
        {
            int? clomint = seatQuantity / 10;
            var colum = "ABCDEF";
            foreach (var VARIABLE in colum)
            {
                clomint--;
                for (int seatRow = 1; seatRow <= 10; seatRow++)
                {
                    var seat = new Seat();
                    seat.CinemaRoomId = cinemaID;
                    seat.SeatColunm=VARIABLE.ToString();
                    seat.SeatRow = seatRow;
                    seat.SeatStatus = "0";
                    seat.seatType_id = 1;
                    _context.Seats.Add(seat);
                    
                }
                if(clomint == 0) break;
            }
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
