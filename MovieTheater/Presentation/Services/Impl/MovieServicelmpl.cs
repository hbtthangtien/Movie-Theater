using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebAPI.Entity;
using WebAPI.Repository;
using WebAPI.Services.DTO;
using WebAPI.Services.DTO.Response;
using Type = WebAPI.Entity.Type;

namespace WebAPI.Services.Impl
{
    public class MovieServicelmpl :GenericServices, IMovieService
    {
        private readonly Cloudinary _cloudinary;
        public MovieServicelmpl(IMapper mapper,Cloudinary cloudinary, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
        {
            _cloudinary = cloudinary;
        }

        public async Task<MovieDTO> GetMovieById(String id)
        {
            var movie = await _context.Movies.GetMovieByIDAsync(id);
            var movieDTO = _mapper.Map<MovieDTO>(movie);
            return movieDTO;
        }

        public async Task<List<MovieDTO>> GetAllMovies()
        {
            var movies = await _context.Movies.GetAllMovies();
            var re= _mapper.Map<List<MovieDTO>>(movies);
            int count=re.Count();
            for (int i = 0; i < count; i++)
            {
                var movie = await _context.Movies.GetMovieByIDAsync(re[i].MovieId);
                var movieDTO = _mapper.Map<MovieDTO>(movie);
                re[i]=movieDTO;
            }

            return re;
        }

        public async Task<List<MovieDTO>> GetMovieByName(string name)
        {
            var movies = _context.Movies.GetMovieByName(name);
            return _mapper.Map<List<MovieDTO>>(movies);
        }

        public async Task<List<MovieDTO>> GetMovieInComing()
        {
            var movies = await _context.Movies.GetMovieInComing();
            var re= _mapper.Map<List<MovieDTO>>(movies);
            int count=re.Count();
            for (int i = 0; i < count; i++)
            {
                var movie = await _context.Movies.GetMovieByIDAsync(re[i].MovieId);
                var movieDTO = _mapper.Map<MovieDTO>(movie);
                re[i]=movieDTO;
            }

            return re;
        }


        public async Task<List<MovieDTO>> GetMovieUpComing()
        {
            var movies = await _context.Movies.GetMovieUpcoming();
            var re= _mapper.Map<List<MovieDTO>>(movies);
            int count=re.Count();
            for (int i = 0; i < count; i++)
            {
                var movie = await _context.Movies.GetMovieByIDAsync(re[i].MovieId);
                var movieDTO = _mapper.Map<MovieDTO>(movie);
                re[i]=movieDTO;
            }

            return re;
        }

        public async Task<bool> UpdateMovie(MovieDTO movieDTO, IFormFile? largeImage, IFormFile? smallImage)
        {
            // Sử dụng FluentValidation ngay trong hàm
            var validator = new MovieDTOValidator();
            var validationResult = validator.Validate(movieDTO);

            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException($"Validation failed: {errors}");
            }

            // Upload ảnh lớn lên Cloudinary
            if (largeImage != null)
            {
                var uploadResult = await UploadImageAsync(largeImage);
                if (uploadResult != null)
                {
                    movieDTO.LargeImage = uploadResult;
                }
            }

            // Upload ảnh nhỏ lên Cloudinary
            if (smallImage != null)
            {
                var uploadResult = await UploadImageAsync(smallImage);
                if (uploadResult != null)
                {
                    movieDTO.SmallImage = uploadResult;
                }
            }

            // Map DTO to entity and update
            var movie = _mapper.Map<Movie>(movieDTO);
            var result = await _context.Movies.UpdateMovieAsync(movie);

            return result;
        }


        
        public async Task<bool> DeleteMovieAsync(string movieId)
        {
            var movie = await _context.Movies.DeleteMovieAsync(movieId);
            return movie;
        }

        public async Task<(bool Success, string? Errors)> AddMovieAsync(ResponseDTOMovie movie, IFormFile? largeImage, IFormFile? smallImage)
        {
            var validator = new ResponseDTOMovieValidator();
            var validationResult = validator.Validate(movie);

            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return (false, errors);
            }

            // Upload ảnh lớn lên Cloudinary
            if (largeImage != null)
            {
                var uploadResult = await UploadImageAsync(largeImage);
                if (uploadResult != null)
                {
                    movie.LargeImage = uploadResult;
                }
            }

            // Upload ảnh nhỏ lên Cloudinary
            if (smallImage != null)
            {
                var uploadResult = await UploadImageAsync(smallImage);
                if (uploadResult != null)
                {
                    movie.SmallImage = uploadResult;
                }
            }

            // Map DTO to entity and save
            var newmovie = _mapper.Map<Movie>(movie);
            var result = await _context.Movies.AddMovieAsync(newmovie);

            // Add movie types
            
            foreach (var type in movie.MovieTypes)
            {
                var check = await _context.Movies.addMovieType(result, type);
            }

            return (true, null);
        }


        private async Task<string?> UploadImageAsync(IFormFile? imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return null; // Không thực hiện upload nếu imageFile là null hoặc rỗng
            }

            using var stream = imageFile.OpenReadStream();
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(imageFile.FileName, stream),
                Folder = "movies"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return uploadResult.SecureUrl.ToString();
            }

            return null; // Trả về null nếu upload không thành công
        }
        public async Task<(List<MovieDTO> Movies, int TotalCount,int TotalPages)> GetMoviesPagedAsync(string? search, int page, int pageSize,int status)
        {
            if (page < 1 || pageSize < 1)
                throw new ArgumentException("Page and pageSize must be greater than 0.");

            // Lấy danh sách phim hoặc tìm kiếm theo tên
            var query = string.IsNullOrEmpty(search)
                ? _context.Movies.GetAllMoviesQueryable()
                : _context.Movies.GetMoviesByNameQueryable(search);

            // Đếm tổng số phim phù hợp
           
            var date = DateOnly.FromDateTime(DateTime.Now);
            switch (status)
            {
                case 1: // Đang chiếu
                    query = query.Where(m => m.FromDate <= date && m.ToDate >= date);
                    break;
                case 2: // Sắp chiếu
                    query = query.Where(m => m.FromDate > date);
                    break;
                default: // Tất cả
                    break;
                    
            }
            int totalCount = await query.CountAsync();

            // Tính số lượng trang tối đa
            int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            
            // Kiểm tra nếu số trang yêu cầu vượt quá giới hạn
            if (page > totalPages && totalCount > 0)
                throw new ArgumentException($"Page {page} exceeds the total number of pages {totalPages}.");

            // Phân trang
            var movies = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            // Chuyển đổi sang DTO
            var movieDTOs = _mapper.Map<List<MovieDTO>>(movies);
            int count=movieDTOs.Count();
            for (int i = 0; i < count; i++)
            {
                var movie = await _context.Movies.GetMovieByIDAsync(movieDTOs[i].MovieId);
                var movieDTO = _mapper.Map<MovieDTO>(movie);
                movieDTOs[i]=movieDTO;
            }
            return (movieDTOs, totalCount,totalPages);
        }

        public async Task<(List<MovieDTO> Movies, int TotalCount, int TotalPages)> GetMoviesincomingAsync(string? search, int page, int pageSize)
        {
            if (page < 1 || pageSize < 1)
                throw new ArgumentException("Page and pageSize must be greater than 0.");

            // Lấy danh sách phim hoặc tìm kiếm theo tên
            var query = string.IsNullOrEmpty(search)
                ? _context.Movies.GetIncomingMoviesQueryable()
                : _context.Movies.GetMoviesByNameDateQueryable(search);

            // Đếm tổng số phim phù hợp
            int totalCount = await query.CountAsync();

            // Tính số lượng trang tối đa
            int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            // Kiểm tra nếu số trang yêu cầu vượt quá giới hạn
            if (page > totalPages && totalCount > 0)
                throw new ArgumentException($"Page {page} exceeds the total number of pages {totalPages}.");

            // Phân trang
            var movies = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            
            // Chuyển đổi sang DTO
            var movieDTOs = _mapper.Map<List<MovieDTO>>(movies);
            int count=movieDTOs.Count();
            for (int i = 0; i < count; i++)
            {
                var movie = await _context.Movies.GetMovieByIDAsync(movieDTOs[i].MovieId);
                var movieDTO = _mapper.Map<MovieDTO>(movie);
                movieDTOs[i]=movieDTO;
            }
            return (movieDTOs, totalCount,totalPages);
        }
    }
        
    
}