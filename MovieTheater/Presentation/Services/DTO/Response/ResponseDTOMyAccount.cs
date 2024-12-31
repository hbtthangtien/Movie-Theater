namespace WebAPI.Services.DTO.Response
{
    public class ResponseDTOMyAccount
    {
        public string AccountId { get; set; }
        public string Username { get; set; }
        public string? Fullname { get; set; }
        public string? Email { get; set; }
        public string IdentityCard { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Gender { get; set; }
        public double? Score { get; set; }
        public string? Image { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public DateOnly? RegisterDate { get; set; }
    }
}
