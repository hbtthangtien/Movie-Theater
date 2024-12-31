using WebAPI.Entity;

namespace WebAPI.Repository
{
    public interface IMemberRepository : IGenericRepository<Member>
    {
        Task<Member> GetMember(string id);
        Task<Member> GetMemberByAccountId(string id);
        public Task<double> getScoreById(string AccountId);
        public Task UpdateScoreById(string AccountId, string InvoiceId);
    }
}
