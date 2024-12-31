using CloudinaryDotNet;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebAPI.Entity;
using Type = System.Type;

namespace WebAPI.Repository.Impl
{
    public class MovieRepositoryImpl:GenericRepositoryImpl<Movie>, IMovieRepository
    {
        private readonly Cloudinary _cloudinary;
        public MovieRepositoryImpl(MovieTheaterContext context , Cloudinary cloudinary) : base(context) { }

        
        

        public async Task<List<Movie>> GetAllMovies()
        {
            var movies = await _context.Movies.ToListAsync();
            if (movies != null)
            {
                return movies;
            }
            else
            {
                return null;
            }
        }

        public List<Movie> GetMovieByName(string name)
        {
            var movies = _context.Movies.Where(m=>m.MovieNameEnglish.Contains(name)||m.MovieNameVn.Contains(name)).ToList();
            if (movies != null)
            {
                return movies;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<Movie>> GetMovieInComing()
        {
            var date=DateOnly.FromDateTime(DateTime.Now);
            var movie = await _context.Movies.Where(d=>d.FromDate<=date&&d.ToDate>=date).ToListAsync();
            
            return movie;
        }

        public async Task<List<Movie>> GetMovieUpcoming()
        {
            var date=DateOnly.FromDateTime(DateTime.Now);
            var movie= await _context.Movies.Where(d => d.FromDate > date).ToListAsync();
            if (movie != null)
            {
                return movie;
            }

            else
            {
                return null;
            }
        }

        public List<string>? getMovieTypeNameByMovieID(string movieID)
        {
            var moviename = _context.Movies.Include(m => m.Types).FirstOrDefault(m=>m.MovieId==movieID);
            var typename = moviename.Types.Select(t => t.TypeName).ToList();
            return typename;
        }

        public async Task<Movie> GetMovieByIDAsync(string ID)
        {
            var movie = await _context.Movies.Include(m=>m.Types).FirstOrDefaultAsync(m => m.MovieId == ID);
            return movie;
        }

        public async Task<bool> DeleteMovieAsync(string movieId)
        {
            // Kiểm tra xem movie có tồn tại hay không
            var movie = _context.Movies.FirstOrDefault(m => m.MovieId == movieId);
            if (movie == null)
                return false;

            // Xóa các dòng liên quan trong bảng Movie_Type bằng SQL trực tiếp
            await _context.Database.ExecuteSqlRawAsync(
                "DELETE FROM Movie_Type WHERE movie_id = {0}", movieId);
            await _context.Database.ExecuteSqlRawAsync(
                "DELETE FROM Movie_Schedule WHERE movie_id = {0}", movieId);
            await _context.Database.ExecuteSqlRawAsync(
                "DELETE FROM Schedule_Seat WHERE movie_id = {0}", movieId);
            // Xóa Movie
            _context.Movies.Remove(movie);

            // Commit thay đổi sau cùng
            await _context.SaveChangesAsync();

            return true;
        }

        public string? getTypeName( int type)
        {
            var  name= _context.Types.Where(t=>t.TypeId==type).FirstOrDefault().TypeName;
            return name;
        }


        public async Task<bool> UpdateMovieAsync(Movie movie)
        {
            var movieUpdate = await _context.Movies
                .Include(m => m.Types) // Bao gồm bảng trung gian
                .FirstOrDefaultAsync(m => m.MovieId == movie.MovieId);
            if (movieUpdate != null)
            {
                // Cập nhật các thuộc tính
                _context.Entry(movieUpdate).CurrentValues.SetValues(movie);

                // Lấy danh sách `TypeId` hiện tại và mới
                var currentTypeIds = movieUpdate.Types.Select(t => t.TypeId).ToHashSet();
                var newTypeIds = movie.Types.Select(t => t.TypeId).ToHashSet();

                // Loại bỏ những `Type` không còn trong danh sách mới
                foreach (var typeId in currentTypeIds.Except(newTypeIds))
                {
                    var movieType = movieUpdate.Types.FirstOrDefault(t => t.TypeId == typeId);
                    if (movieType != null)
                        movieUpdate.Types.Remove(movieType); // Loại bỏ quan hệ
                }

                // Thêm mới những `Type` chưa tồn tại
                foreach (var typeId in newTypeIds.Except(currentTypeIds))
                {
                    await _context.Database.ExecuteSqlRawAsync
                        ("Insert into Movie_Type(movie_id, type_id) values ({0}, {1})", movie.MovieId, typeId);
                }

                await _context.SaveChangesAsync(); // Lưu thay đổi
                return true;
            }

            return false;
        }
        public string GenerateMovieId()
        {
            return $"MV-{Guid.NewGuid().ToString().Substring(0, 6)}"; // Tạo ID dựa trên GUID (6 ký tự đầu)
        }



        public async Task<string> AddMovieAsync(Movie movie)
        {
            while (true)
            {
                movie.MovieId = GenerateMovieId();

                try
                {
                    // Thêm movie mới vào DbSet
                    await _context.Movies.AddAsync(movie);

                    // Lưu thay đổi
                    await _context.SaveChangesAsync();

                    return movie.MovieId; // Thêm thành công
                }
                catch (DbUpdateException ex)
                {
                    // Kiểm tra nếu lỗi là do trùng lặp MovieId
                    if (ex.InnerException?.Message.Contains("UNIQUE KEY constraint") == true)
                    {
                        continue; // Tạo MovieId mới và thử lại
                    }
                    throw; // Nếu lỗi khác, ném lỗi ra ngoài
                }
            }
        }


        public async Task<bool> addMovieType(string movieID, int typeID)
        {
            
                await _context.Database.ExecuteSqlRawAsync
                    ("Insert into Movie_Type(movie_id, type_id) values ({0}, {1})", movieID, typeID);
                
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> updateMovieroom(string movieID, int roomId)
        {
            var existingMovie = await _context.Movies
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.MovieId == movieID);
            return existingMovie != null;
        }
        public IQueryable<Movie> GetAllMoviesQueryable()
        {
            return _context.Movies.AsQueryable();
        }

        public IQueryable<Movie> GetMoviesByNameQueryable(string name)
        {
            return _context.Movies.Where(m => m.MovieNameEnglish.Contains(name)).AsQueryable();
        }
        public IQueryable<Movie> GetIncomingMoviesQueryable()
        {
            var date=DateOnly.FromDateTime(DateTime.Now);
            return _context.Movies.Where(d=>d.FromDate<=date&&d.ToDate>=date).AsQueryable();
        }
        public IQueryable<Movie> GetMoviesByNameDateQueryable(string name)
        {
            var date=DateOnly.FromDateTime(DateTime.Now);
            return _context.Movies.Where(m => m.MovieNameEnglish.Contains(name) &&m.FromDate<=date&&m.ToDate>=date).AsQueryable();
        }

        public async Task<Movie> getMovieByMovieName(string name)
        {
            var movies = await _context.Movies.Where(m => m.MovieNameEnglish.Contains(name) || m.MovieNameVn.Contains(name)).FirstOrDefaultAsync();
            if (movies != null)
            {
                return movies;
            }
            else
            {
                return null;
            }
        }
    }
}