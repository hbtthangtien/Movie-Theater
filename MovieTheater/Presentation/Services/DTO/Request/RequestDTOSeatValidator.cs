using FluentValidation;
using WebAPI.Services.DTO.Request;

public class RequestDTOSeatValidator : AbstractValidator<RequestDTOSeat>
{
    public RequestDTOSeatValidator()
    {
        // Kiểm tra SeatId
        // RuleFor(seat => seat.SeatId)
        //     .GreaterThan(0).WithMessage("SeatId must be greater than 0.");

        // Kiểm tra CinemaRoomId
        RuleFor(seat => seat.CinemaRoomId)
            .NotNull().WithMessage("CinemaRoomId is required.")
            .GreaterThan(0).WithMessage("CinemaRoomId must be greater than 0.");

        // Kiểm tra SeatColunm
        RuleFor(seat => seat.SeatColunm)
            .NotEmpty().WithMessage("SeatColunm is required.")
            .MaximumLength(5).WithMessage("SeatColunm cannot exceed 5 characters.");

        // Kiểm tra SeatRow
        RuleFor(seat => seat.SeatRow)
            .NotNull().WithMessage("SeatRow is required.")
            .GreaterThan(0).WithMessage("SeatRow must be greater than 0.")
            .LessThanOrEqualTo(100).WithMessage("SeatRow cannot exceed 100.");

        // Kiểm tra SeatStatus
        RuleFor(seat => seat.SeatStatus)
            .NotNull().WithMessage("SeatStatus is required.")
            .InclusiveBetween(0, 1).WithMessage("SeatStatus must be either 0 (inactive) or 1 (active).");

        // Kiểm tra seatType_id
        RuleFor(seat => seat.seatType_id)
            .NotNull().WithMessage("seatType_id is required.")
            .GreaterThan(0).WithMessage("seatType_id must be greater than 0.");
    }
}