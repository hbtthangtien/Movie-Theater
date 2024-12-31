namespace WebAPI.Services.DTO.Response
{
    public class ResponseDTOPayment
    {
        public string message { get; set; }

        public string InvoiceId { get; set; }

        public double Amount { get; set; }

        public string Status { get; set; }
    }
}
