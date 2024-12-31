using WebAPI.Entity;

namespace WebAPI.Services.DTO.Response
{
    public class ResponseDTOLogin
    {
        public string AccountId { get; set; } = null!;

        public string? Address { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public string? Email { get; set; }

        public string? Fullname { get; set; }

        public string? Gender { get; set; }

        public string? IdentityCard { get; set; }

        public string? Image { get; set; }

        public string? PhoneNumber { get; set; }

        public DateOnly? RegisterDate { get; set; }

        public int? RoleId { get; set; }

        public int? Status { get; set; }

        public string? Username { get; set; }

       
    }
}
