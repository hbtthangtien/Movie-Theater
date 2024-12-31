using AutoMapper;
using FluentValidation;
using WebAPI.Entity;
using WebAPI.Repository;
using WebAPI.Services.DTO.Request;

namespace WebAPI.Services.Impl;

public class SeatServicesImpl:GenericServices, ISeatServices
{
    public SeatServicesImpl(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
    {
    }

    public async Task<bool> AddSeat(List<RequestDTOSeat> seats)
    {
        var validator = new RequestDTOSeatValidator();
        foreach (var VARIABLE in seats)
        {
            var validationResult = validator.Validate(VARIABLE);

            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException($"Validation failed: {errors}");
            } 
            break;
        }
        var seate= _mapper.Map<List<Seat>>(seats);
        await _context.Seats.AddSeat(seate);
        return true;
    }

    public async Task<bool> UpdateSeat(List<RequestDTOSeat> seat)
    {
        var validator = new RequestDTOSeatValidator();
        foreach (var VARIABLE in seat)
        {
            var validationResult = validator.Validate(VARIABLE);

            if (!validationResult.IsValid)
            {
                var errors = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException($"Validation failed: {errors}");
            } 
            break;
        }
       
        var seats = _mapper.Map<List<Seat>>(seat);
        await _context.Seats.UpdateSeat(seats);
        return true;
    }

    public async Task<bool> DeleteSeat(List<int> seatIds)
    {
        
        await _context.Seats.DeleteSeat(seatIds);
        return true;
    }

    public async Task<List<RequestDTOSeat>> GetAllSeatByCinemaID(int cinemaID)
    {
        var seat = await _context.Seats.GetAllSeatByCinemaID(cinemaID);
        var seats = _mapper.Map<List<RequestDTOSeat>>(seat);
        return seats;
    }
}