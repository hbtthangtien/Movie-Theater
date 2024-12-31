using Microsoft.AspNetCore.Identity;

namespace WebAPI.Entity
{
    public class ApplicationUser : IdentityUser
    {
        public string? Address { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public string? Email { get; set; }

        public string? Fullname { get; set; }

        public string? Gender { get; set; }

        public string IdentityCard { get; set; } = null!;
        public string? Image { get; set; }
        public string? Password { get; set; }

        public string? PhoneNumber { get; set; }

        public DateOnly? RegisterDate { get; set; }

        public int? Status { get; set; }
        public string? RefreshToken { get; set; }
        public DateOnly? RefreshTokenExpire { get; set; }

        public virtual ICollection<Employee>? Employees { get; set; } = new List<Employee>();

        public virtual ICollection<Invoice>? Invoices { get; set; } = new List<Invoice>();

        public virtual ICollection<Member>? Members { get; set; } = new List<Member>();
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
        public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
        public virtual ICollection<ScheduleSeat> ScheduleSeats { get; set; } = new List<ScheduleSeat>();
    }
}
