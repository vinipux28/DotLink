using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotLink.Api.Models
{
    public class UploadFileRequest
    {
        [FromForm]
        public IFormFile File { get; set; } = default!;
    }
}