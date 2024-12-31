using Microsoft.AspNetCore.Identity;
using WebAPI.Entity;

namespace WebAPI.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> CommitAsync();
        UserManager<ApplicationUser> Accounts { get; }
        IMovieRepository Movies { get; }

        ICinemaRoomRepository CinemaRooms { get; }

        IInvoiceRepository Invoices { get; }

        IMovieScheduleRepository MovieSchedules { get; }

        IPromotionRepository Promotions { get; }

        //IRoleRepository Roles { get; }

        IScheduleRepository Schedules { get; }

        IScheduleSeatRepository ScheduleSeats { get; }

        ISeatRepository Seats { get; }

        ITicketRepository Tickets { get; }

        ITypeRepository TypeRepository { get; }

        IMemberRepository Members { get; }

        IPaymentRepository Payments { get; }

        IEmployeeRepository Employees { get; }
    }
}

