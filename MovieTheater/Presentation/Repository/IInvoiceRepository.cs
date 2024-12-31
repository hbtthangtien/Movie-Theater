using WebAPI.Entity;
using WebAPI.Services.DTO.Response;

namespace WebAPI.Repository
{
    public interface IInvoiceRepository : IGenericRepository<Invoice>
    {
        public List<Invoice> GetInvoiceByAccountIdForBookedTickets(string accountId,string movieName, int page = 1);
        public List<Invoice> GetInvoiceByAccountIdForHistoryScore(DateTime? fromDate, DateTime? toDate, string accountId, string historyType, int page = 1);

        public Task<string> GetUserIdByInvoiceId(string InvoiceId);
    }
}
