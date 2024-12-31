using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Identity.Client;
using System.Linq;
using WebAPI.Entity;
using WebAPI.Exception;
using WebAPI.Repository;
using WebAPI.Services.DTO.Response;

namespace WebAPI.Services.Impl
{
    public class InvoiceServiceImpl : GenericServices, IInvoiceService  
    {
        
        public InvoiceServiceImpl(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork)
        {
        }
        public async Task<string>   CreateInvoiceAsync(string accountId, RequestDTOPayment request,double total_amount)
        {
            var invoice = new Invoice
            {
                InvoiceId = generateId(),
                AddScore = (total_amount / 10000),
                BookingDate = DateTime.Now,
                MovieName = request.MovieName,
                ScheduleShow = request.ScheduleShow,
                ScheduleShowTime = request.ScheduleShowTime,
                AccountId = accountId,
                TotalMoney = (decimal?)total_amount,
                Seat = getSeat(request.ScheduleSeats),
                Status = "Pending",
                cinema_room_name = request.CinemaRoom,
                UseScore = request.UseScore

            };
            await _context.Invoices.AddAsync(invoice);
            await _context.CommitAsync();
            return invoice.InvoiceId;
        }

        public async Task<List<ResponseDTOHistory>> GetInvoiceForHistoryScore(DateTime? fromDate, DateTime? toDate, string userName, string historyType, int page = 1)
        {
            try
            {
                var user = await _context.Accounts.FindByNameAsync(userName) ?? throw new NotFoundException("User not found.");
                var score = await _context.Members.getScoreById(user.Id);
                var invoices = _context.Invoices.GetInvoiceByAccountIdForHistoryScore(fromDate, toDate, user.Id, historyType, page).Select(i => new ResponseDTOHistory
                {
                    MovieName = i.MovieName,
                    BookingDate = i.BookingDate,
                    UseScore = i.UseScore,
                    AddScore = i.AddScore,
                }).ToList();

                return invoices;
            }
            catch (System.Exception e)
            {
                throw new ApplicationException("An error occurred while retrieving score history.", e);
            }
        }

        public async Task<List<ResponseDTOHistory>>GetInvoiceForBookedTicket(string userName, string movieName, int page = 1)
        {
            try
            {
                var user = await _context.Accounts.FindByNameAsync(userName) ?? throw new ApplicationException("User not found.");
                var score = await _context.Members.getScoreById(user.Id);
                var invoices = _context.Invoices.GetInvoiceByAccountIdForBookedTickets(user.Id,movieName, page).Select(i => new ResponseDTOHistory
                {
                    InvoiceId = i.InvoiceId,
                    MovieName = i.MovieName,
                    BookingDate = i.BookingDate,
                    TotalMoney = i.TotalMoney,
                    Status = i.Status,
                    Image = user.Image,
                    AccountId = user.Id,
                    userName = userName,
                    Score = score
                }).ToList();
               
                return invoices;
            }
            catch(System.Exception e) {
                throw new ApplicationException("An error occurred while retrieving invoice history.", e);
            }
        }
        public async Task UpdateStatusInvoice(string InvoiceId, string status)
        {
            var Invoice =await _context.Invoices.GetByIdAsync(InvoiceId)
                 ?? throw new NotFoundException($"There is no Invoice with Id: {InvoiceId}");
            Invoice.Status = status;
            _context.Invoices.Update(Invoice);
            await _context.CommitAsync();
        }

        private string generateId()
        {
            Guid guid = Guid.NewGuid();
            string uniqueShortGuid = guid.ToString("N").Substring(0, 6) + DateTime.UtcNow.Ticks.ToString().Substring(10, 4);
            return uniqueShortGuid;
        }
        private string getSeat(ICollection<RequestDTOPayment.RequestDTOScheduleSeat> list)
        {
            string ans = "";
            foreach(var i in list)
            {
                ans += i.SeatRow + i.SeatColumn;
                ans += " ";
            }
            return ans;
        }
        public async Task<string> GetUserIdByInvoiceId(string InvoiceId)
        {
            return await _context.Invoices.GetUserIdByInvoiceId(InvoiceId);
        }

        public async Task<ResponseDTOHistory> ViewDetailInvoice(string InvoiceId)
        {
            var invoice = await _context.Invoices.GetByIdAsync(InvoiceId) ?? throw new NotFoundException("Invoice not found.");

            var movie = await _context.Movies.getMovieByMovieName(invoice.MovieName);
            return new ResponseDTOHistory
            {
                MovieName = invoice.MovieName,
                cinema_room_name = invoice.cinema_room_name,
                BookingDate = invoice.BookingDate,
                ScheduleShowTime = invoice.ScheduleShowTime,
                Seat = invoice.Seat,
                UseScore = invoice.UseScore,
                AddScore = invoice.AddScore,
                Status = invoice.Status,
                TotalMoney = invoice.TotalMoney,
                Image = movie.SmallImage,
            };
        }
    }
}
