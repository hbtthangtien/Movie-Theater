using FluentValidation;
using WebAPI.Services.DTO.Request;

public class ResquestDTOMovieRoomValidator : AbstractValidator<ResquestDTOMovieRoom>
{
    public ResquestDTOMovieRoomValidator()
    {
        // RuleFor(room => room.CinemaRoomId)
        //     .GreaterThan(0).WithMessage("CinemaRoomId must be greater than 0.");

        RuleFor(room => room.CinemeRoomName)
            .NotEmpty().WithMessage("CinemeRoomName is required.")
            .MaximumLength(100).WithMessage("CinemeRoomName cannot exceed 100 characters.");

        RuleFor(room => room.SeatQuantity)
            .NotNull().WithMessage("SeatQuantity is required.")
            .GreaterThan(0).WithMessage("SeatQuantity must be greater than 0.")
            .LessThanOrEqualTo(500).WithMessage("SeatQuantity cannot exceed 500.");
    }
}