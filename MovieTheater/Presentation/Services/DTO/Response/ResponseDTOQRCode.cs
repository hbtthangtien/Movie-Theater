namespace WebAPI.Services.DTO.Response
{
    public class ResponseDTOQRCode
    {
        public string code {  get; set; }
        public string description { get; set; }
        public Data? data { get; set; }

        public class Data
        {
            public string? QrCode { get; set; }

            public string? QrDataUrl { get; set; }
        }
    }
}
