using Microsoft.AspNetCore.Mvc;
using DotLink.Application.Services;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mime;

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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFile(string fileKey)
        {
            if (string.IsNullOrWhiteSpace(fileKey)) return BadRequest("File key cannot be empty.");

            try
            {
                var fileStream = await _fileStorageService.GetFileStreamAsync(fileKey);
                return File(fileStream, GetContentType(fileKey));
            }
            catch (FileNotFoundException ex)
            {
                return NotFound($"Filename: {ex.FileName}");
            }
        }

        private static string GetContentType(string fileKey)
        {
            var extension = Path.GetExtension(fileKey)?.ToLowerInvariant();
            return extension switch
            {
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".pdf" => "application/pdf",
                _ => MediaTypeNames.Application.Octet,
            };
        }
    }
}