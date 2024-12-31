namespace WebAPI.Services
{
    public interface ITicketService
    {
        public Task CreateTicketAsync(string AccountId, RequestDTOPayment request);
        public Task CreateTickByEmployeeAsync(ICollection<RequestDTOPayment.RequestDTOScheduleSeat> ScheduleSeats, string InvoiceID);
    }
}
