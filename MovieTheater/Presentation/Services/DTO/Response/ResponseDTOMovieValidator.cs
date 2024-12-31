using FluentValidation;
using WebAPI.Services.DTO.Response;

public class ResponseDTOMovieValidator : AbstractValidator<ResponseDTOMovie>
{
    public ResponseDTOMovieValidator()
    {
        RuleFor(movie => movie.MovieId)
            .NotEmpty().WithMessage("MovieId is required.");

        RuleFor(movie => movie.Actor)
            .MaximumLength(100).WithMessage("Actor name cannot exceed 100 characters.");

        // RuleFor(movie => movie.Duration)
        //     .Matches(@"^\d+h\s\d+m$").WithMessage("Duration must be in the format 'Xh Ym'.");

        RuleFor(movie => movie.MovieNameEnglish)
            .MaximumLength(200).WithMessage("Movie name in English cannot exceed 200 characters.");

        RuleFor(movie => movie.MovieNameVn)
            .MaximumLength(200).WithMessage("Movie name in Vietnamese cannot exceed 200 characters.");

        RuleFor(movie => movie.ToDate)
            .GreaterThanOrEqualTo(movie => movie.FromDate)
            .When(movie => movie.FromDate.HasValue && movie.ToDate.HasValue)
            .WithMessage("ToDate must be greater than or equal to FromDate.");

        RuleFor(movie => movie.MovieTypes)
            .NotNull().WithMessage("MovieTypes cannot be null.")
            .Must(types => types.All(t => t > 0))
            .WithMessage("All MovieTypes must be greater than 0.");
    }
}