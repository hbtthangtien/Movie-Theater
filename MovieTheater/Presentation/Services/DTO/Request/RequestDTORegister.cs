namespace WebAPI.Services.DTO.Request
{
    public class RequestDTORegister
    {
        public string? AccountId { get; set; }
        public string? Address { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public string? Email { get; set; }

        public string? Fullname { get; set; }

        public string? Gender { get; set; }

        public string? IdentityCard { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Username { get; set; }

        public int? RoleId { get; set; }
    }
}
