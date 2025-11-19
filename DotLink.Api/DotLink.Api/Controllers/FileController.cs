using Microsoft.AspNetCore.Mvc;
using DotLink.Application.Services;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mime;
using System.IO;

namespace DotLink.Api.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IFileStorageService _fileStorageService;

        public FileController(IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }

        [HttpGet("{**fileKey}")]
        public async Task<IActionResult> GetFile(string fileKey)
        {
            if (string.IsNullOrWhiteSpace(fileKey))
            {
                return BadRequest("File key cannot be empty.");
            }

            try
            {
                var fileStream = await _fileStorageService.GetFileStreamAsync(fileKey);

                var contentType = GetContentType(fileKey);

                return File(fileStream, contentType);
            }
            catch (FileNotFoundException ex)
            {
                return NotFound($"Filename: {ex.FileName}");
            }
        }


        private string GetContentType(string fileKey)
        {
            var extension = Path.GetExtension(fileKey)?.ToLowerInvariant();
            return extension switch
            {
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".jpg" or ".jpeg" => "image/jpeg",
                _ => MediaTypeNames.Application.Octet,
            };
        }
    }
}