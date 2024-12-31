using Microsoft.EntityFrameworkCore;
using WebAPI.Entity;
using WebAPI.Exception;

namespace WebAPI.Repository.Impl
{
    public class MemberRepositoryImpl : GenericRepositoryImpl<Member>, IMemberRepository
    {
        public MemberRepositoryImpl(MovieTheaterContext context) : base(context)
        {
        }

        public async Task<Member> GetMember(string id)
        {
            var c = await _context.Members.FirstOrDefaultAsync(member => member.MemberId == id);
            return c;

        }

        public async Task<Member> GetMemberByAccountId(string id)
        {
            var member = await _context.Members.Include(m => m.Account).FirstOrDefaultAsync(m => m.AccountId == id);
            return member;
        }

        public async Task<double> getScoreById(string AccountId)
        {
            var member = await _context.Members.FirstOrDefaultAsync(m => m.AccountId == AccountId)
                        ?? throw new NotFoundException($"No member with AccountId = {AccountId} existed!!!");
            return (double)member.Score!;
        }

        public async Task UpdateScoreById(string AccountId, string InvoiceId)
        {
            var u = await _context.Members.FirstOrDefaultAsync(e => e.MemberId == AccountId || e.AccountId == AccountId)
                    ?? throw new NotFoundException($"No member with AccountId = {AccountId} existed!!!");
            var Invoice = await _context.Invoices.FindAsync(InvoiceId)
            ?? throw new NotFoundException($"No member with InvoiceID = {InvoiceId} existed!!!");
            u.Score -= Invoice.UseScore;
            if (u.Score < 0) throw new System.Exception("The value to UseScore is not Invalid!!!");
            u.Score += Invoice.AddScore;
            _context.Members.Update(u);
            await _context.SaveChangesAsync();
        }
    }
}
