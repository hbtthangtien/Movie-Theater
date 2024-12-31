using WebAPI.Services.DTO.Request;
using WebAPI.Services.DTO.Response;

namespace WebAPI.Services
{
    public interface IPaymentService
    {
        public Task<string> CreatePaymentAsync(RequestDTOPayment request,HttpContext context, string username);

        public Task<ResponseDTOPayment> ExecutePayment(IQueryCollection collection);

        public Task<ResponseDTOPaymentEmployee> CreatePaymentForEmployeeAsync(RequestDTOPaymentEmployee request, string username);

        public Task<ResponseDTOApi> ComfirmPaymentForEmployee(RequestDTOPaymentEmployee request, string username);

        public Task<ResponseDTOApi> GenerateTicket(RequestDTOPayment request, string username);

    }
    
}
