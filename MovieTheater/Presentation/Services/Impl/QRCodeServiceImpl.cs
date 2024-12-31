using AutoMapper;
using System.Text;
using System.Text.Json;
using WebAPI.Repository;
using WebAPI.Services.DTO.Request;
using WebAPI.Services.DTO.Response;

namespace WebAPI.Services.Impl
{
    public class QRCodeServiceImpl : GenericServices, IQRCodeServicee
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public QRCodeServiceImpl(IMapper mapper, IUnitOfWork unitOfWork
            , IHttpClientFactory httpClientFactory) : base(mapper, unitOfWork)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ResponseDTOQRCode?> GenerateVietQRCodeAsync(RequestDTOQRCode request)
        {
            var client = _httpClientFactory.CreateClient();
            var url = "https://api.vietqr.io/v2/generate";
            var requestBody = new
            {
                accountNo = long.Parse("4271049042"),
                accountName = "Movie Theater Systemm",
                acqId = Convert.ToInt32("970418"),
                amount = Convert.ToInt32(request.Amount),
                addInfo = request.AddInfor,
                format = "text",
                template = "compact2"
            };
            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            if(response.IsSuccessStatusCode)
            {
                var qrcode = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions()
                {
                    PropertyNameCaseInsensitive = true,
                };
                return JsonSerializer.Deserialize<ResponseDTOQRCode>(qrcode, options);
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Failed to generate VietQR: {errorContent}");
            }
        }
    }
}
