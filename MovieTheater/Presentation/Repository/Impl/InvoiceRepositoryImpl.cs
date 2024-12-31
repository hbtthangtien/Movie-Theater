using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WebAPI.Entity;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebAPI.Repository.Impl
{

    public class InvoiceRepositoryImpl : GenericRepositoryImpl<Invoice>, IInvoiceRepository
    {
        public static int pageNumber { get; set; } = 5;
        public InvoiceRepositoryImpl(MovieTheaterContext context) : base(context)
        {
        }
        

        public List<Invoice> GetInvoiceByAccountIdForBookedTickets(string accountId, string movieName, int page = 1)
        {

            var invoices = _context.Invoices
                         .Where(i => i.AccountId == accountId);
            if(string.IsNullOrEmpty(movieName) != true)
            {
                invoices = _context.Invoices
                         .Where(i => i.AccountId == accountId && i.MovieName.Contains(movieName));
            }
           invoices = invoices.Skip((page - 1) * pageNumber).Take(pageNumber)
                         .OrderByDescending(i => i.BookingDate);
            return invoices.ToList();

        }

        public List<Invoice> GetInvoiceByAccountIdForHistoryScore(DateTime? fromDate, DateTime? toDate, string accountId, string historyType,  int page = 1)
        {
            var invoices = _context.Invoices
                        .Where(i => i.AccountId == accountId && i.Status != "Cancelled");
            if(fromDate != null && toDate != null)
            {
                invoices = invoices.Where(i => i.BookingDate >= fromDate && i.BookingDate <= toDate);
            }
            else if(fromDate != null)
            {
                invoices = invoices.Where(i => i.BookingDate >= fromDate);
            }
            else if(toDate != null)
            {
                invoices = invoices.Where(i => i.BookingDate <= toDate);
            }

            if (historyType == "adding")
            {
                invoices = invoices.Where(i => i.AddScore > 0);
            }
            else if (historyType == "using")
            {
                invoices = invoices.Where(i => i.UseScore > 0);
            }
            invoices = invoices.Skip((page - 1) * pageNumber).Take(pageNumber)
                         .OrderByDescending(i => i.BookingDate);

            return invoices.ToList();
        }

        public async Task<string> GetUserIdByInvoiceId(string InvoiceId)
        {
            var Invoice = await _context.Invoices.FirstOrDefaultAsync(e => e.InvoiceId == InvoiceId)
                ?? throw new DllNotFoundException($"Not Found Invoice with ID: {InvoiceId}");
            return Invoice.AccountId!;
        }
    }
}
