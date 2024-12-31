namespace WebAPI.Services.DTO.Request
{
    public class RequestDTOConfirm
    {
        public string? InvoiceID { get; set; }

        public string? MemberParameter { get; set; }

        public bool Confirm {  get; set; }
    }
}
