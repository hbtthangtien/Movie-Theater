using WebAPI.Entity;

namespace WebAPI.Repository.Impl
{
    public class TicketRepositoryImpl : GenericRepositoryImpl<Ticket>, ITicketRepository
    {
        public TicketRepositoryImpl(MovieTheaterContext context) : base(context)
        {
        }
    }
}
