namespace WebAPI.Services.DTO.Request
{
    public class RequestDTOEmployeeManagement
    {
        public string? Fullname { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? Gender { get; set; }
    }
}
