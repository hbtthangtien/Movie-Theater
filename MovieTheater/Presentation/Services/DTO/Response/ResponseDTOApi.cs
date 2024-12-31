using System.Net;

namespace WebAPI.Services.DTO.Response
{
    public class ResponseDTOApi
    {
        public ResponseDTOApi()
        {
            errors = new List<string>();
        }
        public HttpStatusCode StatusCode { get; set; }

        public string? StatusMessage { get; set; }

        public List<string> errors { get; set; }

        
    }
}
