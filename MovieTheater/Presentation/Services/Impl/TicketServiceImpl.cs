using AutoMapper;
using Azure.Core;
using Microsoft.Identity.Client;
using System.Text.RegularExpressions;
using WebAPI.Entity;
using WebAPI.Exception;
using WebAPI.Repository;

namespace WebAPI.Services.Impl
{
    public class TicketServiceImpl : GenericServices, ITicketService
    {
        private readonly IScheduleSeatService _scheduleSeatService;
        public TicketServiceImpl(IMapper mapper, IUnitOfWork unitOfWork, IScheduleSeatService scheduleSeatService) : base(mapper, unitOfWork)
        {
            _scheduleSeatService = scheduleSeatService;
        }
        public async Task CreateTickByEmployeeAsync(ICollection<RequestDTOPayment.RequestDTOScheduleSeat> ScheduleSeats, string InvoiceID)
        {
            var Invoice = await _context.Invoices.GetByIdAsync(InvoiceID)
                        ?? throw new NotFoundException($"There is no Invoice with ID: {InvoiceID}");
            foreach (var i in ScheduleSeats)
            {
                var scheduleSeat = await _context.ScheduleSeats.GetByIdAsync(i.ScheduleSeatId!)
                    ?? throw new NotFoundException($"Schedule Seat with Id: {i.ScheduleSeatId} is not exist!!!");                
                var ticket = new Ticket
                {
                    AccountId = Invoice.AccountId,
                    Price = (decimal?)i.Type.Price,
                    ScheduleSeatId = i.ScheduleSeatId,
                    cinema_room_name = Invoice.cinema_room_name,
                    movie_name = Invoice.MovieName,
                    schedule_show = Invoice.ScheduleShow,
                    schedule_show_time = Invoice.ScheduleShowTime,
                    TicketType = 1

                };
                await _context.Tickets.AddAsync(ticket);
                await _scheduleSeatService.SelectedSeat(i.ScheduleSeatId);

            }
            await _context.CommitAsync();
        }

        public async Task CreateTicketAsync(string AccountId, RequestDTOPayment request)
        {
            foreach (var i in request.ScheduleSeats)
            {
                var scheduleSeat = await _context.ScheduleSeats.GetByIdAsync(i.ScheduleSeatId)
                    ?? throw new NotFoundException($"Schedule Seat with Id: {i.ScheduleSeatId} is not exist!!!");
                var ticket = new Ticket
                {
                    AccountId = AccountId,
                    Price = (decimal?)i.Type.Price,
                    ScheduleSeatId = i.ScheduleSeatId,
                    cinema_room_name = request.CinemaRoom,
                    movie_name = request.MovieName,
                    schedule_show = request.ScheduleShow,
                    schedule_show_time =  request.ScheduleShowTime,
                    TicketType = 1

                };
                await _context.Tickets.AddAsync(ticket);
                await _scheduleSeatService.SelectedSeat(i.ScheduleSeatId);
            }
            await _context.CommitAsync();

        }
    }
}
