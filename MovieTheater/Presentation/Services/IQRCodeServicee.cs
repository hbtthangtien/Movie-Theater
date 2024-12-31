using WebAPI.Services.DTO.Request;
using WebAPI.Services.DTO.Response;

namespace WebAPI.Services
{
    public interface IQRCodeServicee
    {
        public Task<ResponseDTOQRCode> GenerateVietQRCodeAsync(RequestDTOQRCode request);
    }
}
