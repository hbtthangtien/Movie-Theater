using WebAPI.Entity;

namespace WebAPI.Repository.Impl
{
    public class PaymentRepositoryImpl : GenericRepositoryImpl<Payment>, IPaymentRepository
    {
        public PaymentRepositoryImpl(MovieTheaterContext context) : base(context)
        {
        }
    }
}
