using WebAPI.Entity;
using WebAPI.Services.DTO.Response;

namespace WebAPI.Services
{
    public interface IInvoiceService
    {
        public Task<List<ResponseDTOHistory>> GetInvoiceForBookedTicket(string userName, string movieName, int page = 1);
        public Task<List<ResponseDTOHistory>> GetInvoiceForHistoryScore(DateTime? fromDate, DateTime? toDate, string userName, string historyType, int page = 1);
        public Task<string> CreateInvoiceAsync(string accountId,RequestDTOPayment request, double total_amount);
        public Task UpdateStatusInvoice(string InvoiceId, string status);
        public Task<string> GetUserIdByInvoiceId(string InvoiceId);
        public Task<ResponseDTOHistory> ViewDetailInvoice(string InvoiceId);
    }
}
