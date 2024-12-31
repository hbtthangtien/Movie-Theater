using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebAPI.Repository;
using WebAPI.Services.DTO.Request;
using WebAPI.Services.DTO.Response;
using WebAPI.Entity;
namespace WebAPI.Services.Impl;

public class MovieRoomServiceImpl:GenericServices,IMovieRomeServices
{
    public MovieRoomServiceImpl(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<List<ResquestDTOMovieRoom>> GetAllMovieRoom()
    {
        var movieroom= await _context.CinemaRooms.GetAllAsync();
        return _mapper.Map<List<ResquestDTOMovieRoom>>(movieroom);
    }

    public async Task<ResquestDTOMovieRoom> GetMovieRoomById(int id)
    {
        var movieroom= await _context.CinemaRooms.GetMovieRoomByID(id);
        return _mapper.Map<ResquestDTOMovieRoom>(movieroom);
    }

    public async Task<bool> addMovieroom(ResquestDTOMovieRoom movieroom)
    {
        var validator = new ResquestDTOMovieRoomValidator();
        var validationResult = validator.Validate(movieroom);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new ValidationException($"Validation failed: {errors}");
        }
        var movieroomEntity = _mapper.Map<CinemaRoom>(movieroom);
        var cinema= await _context.CinemaRooms.AddCinemaRoom(movieroomEntity);
        _context.CinemaRooms.autoAddSeat(cinema.CinemaRoomId,movieroomEntity.SeatQuantity);
        return true;
    }

    public async Task<bool> updateMovieroom(ResquestDTOMovieRoom movieroom)
    {
        var validator = new ResquestDTOMovieRoomValidator();
        var validationResult = validator.Validate(movieroom);
        if (!validationResult.IsValid)
        {
            var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
            throw new ValidationException($"Validation failed: {errors}");
        }
        var movieroomEntity = _mapper.Map<CinemaRoom>(movieroom);
        await _context.CinemaRooms.updateinemaRoom(movieroomEntity);
        return true;
    }

    public async Task<bool> deleteMovieroom(int movieroom)
    {
        await _context.CinemaRooms.deleteMovieroom(movieroom);
        return true;
    }
    public async Task<(List<ResquestDTOMovieRoom> MovieRooms, int TotalCount, int TotalPages)> GetMovieRoomsPagedAsync(string? search, int page, int pageSize)
    {
        if (page < 1 || pageSize < 1)
            throw new ArgumentException("Page and pageSize must be greater than 0.");

        // Lấy danh sách phòng chiếu hoặc tìm kiếm theo tên
        var query = string.IsNullOrEmpty(search)
            ? _context.CinemaRooms.GetAllQueryable()
            : _context.CinemaRooms.GetMovieRoomsByName(search);

        // Đếm tổng số phòng chiếu phù hợp
        int totalCount = await query.CountAsync();

        // Tính số lượng trang tối đa
        int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

        // Kiểm tra nếu số trang yêu cầu vượt quá giới hạn
        if (page > totalPages && totalCount > 0)
            throw new ArgumentException($"Page {page} exceeds the total number of pages {totalPages}.");

        // Phân trang
        var movieRooms = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        // Chuyển đổi sang DTO
        var movieRoomDTOs = _mapper.Map<List<ResquestDTOMovieRoom>>(movieRooms);

        return (movieRoomDTOs, totalCount, totalPages);
    }


}