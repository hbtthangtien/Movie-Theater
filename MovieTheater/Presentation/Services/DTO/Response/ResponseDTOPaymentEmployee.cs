namespace WebAPI.Services.DTO.Response
{
    public class ResponseDTOPaymentEmployee
    {
        public string Status { get; set; }
        public string InvoiceId { get; set; }
        public ResponseDTOQRCode QrCode { get; set; }
        public string AccountId { get; set; }
    }
}
