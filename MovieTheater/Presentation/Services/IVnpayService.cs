using WebAPI.Services.DTO.Request;
using WebAPI.Services.DTO.Response;

namespace WebAPI.Services
{
    public interface IVnpayService
    {
        public string CreatePaymentUrl(HttpContext context, RequestDTOVnpay request);

        public ResponseDTOVnpay PaymentExcute(IQueryCollection collections);
    }
}
