namespace WebAPI.Services.DTO
{

    public class MovieDTO
    {
        
        public string MovieId { get; set; } = null!;

        public string? Actor { get; set; }

        public int? CinemaRoomId { get; set; }

        public string? Content { get; set; }

        public string? Director { get; set; }

        public string? Duration { get; set; }

        public DateOnly? FromDate { get; set; }

        public string? MovieProductionCompany { get; set; }

        public DateOnly? ToDate { get; set; }

        public string? Version { get; set; }

        public string? MovieNameEnglish { get; set; }

        public string? MovieNameVn { get; set; }

        public string? LargeImage { get; set; }

        public string? SmallImage { get; set; } 
        public virtual ICollection<TypeDTO> Types { get; set; } = new List<TypeDTO>();
    }
}