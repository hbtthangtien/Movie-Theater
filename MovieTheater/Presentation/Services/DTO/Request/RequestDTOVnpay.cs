using WebAPI.Entity;

namespace WebAPI.Services.DTO.Request
{
    public class RequestDTOVnpay
    {
        public required double TotalAmount { get; set; }

        public required string InvoiceId { get; set; }

        public string? InvoiceMessage {  get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

    }
}
