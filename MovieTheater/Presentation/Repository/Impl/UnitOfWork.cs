using Microsoft.AspNetCore.Identity;
using WebAPI.Entity;

namespace WebAPI.Repository.Impl
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MovieTheaterContext _context;
        //public IAccountRepository Accounts {  get; private set; }

        public IMovieRepository Movies { get; private set; }

        public ICinemaRoomRepository CinemaRooms { get; private set; }

        public IInvoiceRepository Invoices { get; private set; }

        public IMovieScheduleRepository MovieSchedules { get; private set; }

        public IPromotionRepository Promotions { get; private set; }

        public IScheduleRepository Schedules { get; private set; }

        public IScheduleSeatRepository ScheduleSeats { get; private set; }

        public ISeatRepository Seats { get; private set; }

        public ITicketRepository Tickets { get; private set; }

        public ITypeRepository TypeRepository { get; private set; }

        public UserManager<ApplicationUser> Accounts { get; private set; }

        public IMemberRepository Members { get; private set; }

        public IEmployeeRepository Employees { get; private set; }

        public IPaymentRepository Payments { get; private set; }

        public UnitOfWork(MovieTheaterContext context,
            IMovieRepository movies,
            ICinemaRoomRepository cinemaRooms,
            IInvoiceRepository invoices,
            IMovieScheduleRepository movieSchedules,
            IPromotionRepository promotions,
            IScheduleRepository schedules,
            IScheduleSeatRepository scheduleSeats,
            ISeatRepository seats,
            ITicketRepository tickets,
            ITypeRepository typeRepository,
            UserManager<ApplicationUser> userManager,
            IEmployeeRepository employeeRepository,
            IMemberRepository memberRepository,
            IPaymentRepository paymentRepository
            )
        {
            _context = context;
            Movies = movies;
            CinemaRooms = cinemaRooms;
            Invoices = invoices;
            MovieSchedules = movieSchedules;
            Promotions = promotions;
            Schedules = schedules;
            ScheduleSeats = scheduleSeats;
            Seats = seats;
            Tickets = tickets;
            TypeRepository = typeRepository;
            Accounts = userManager;
            Members = memberRepository;
            Employees = employeeRepository;
            Payments = paymentRepository;
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.DisposeAsync();
        }
    }
}
