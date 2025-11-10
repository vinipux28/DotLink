using DotLink.Api.Models;
using DotLink.Application.DTOs;
using DotLink.Application.Features.Users.ChangePassword;
using DotLink.Application.Features.Users.RemoveProfilePicture;
using DotLink.Application.Features.Users.UploadProfilePicture;
using DotLink.Application.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotLink.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserRepository _userRepository;
        public UserController(IMediator mediator, IUserRepository userRepository)
        {
            _mediator = mediator;
            _userRepository = userRepository;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(new UserDTO(user));
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out Guid userId)) return Unauthorized();

            var user = await _userRepository.GetByIdAsync(userId);
            if (user is null)
            {
                return NotFound();
            }
            return Ok(new UserDTO(user));
        }


        [Authorize]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserProfileCommand command)
        {
            if (!Guid.TryParse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value, out Guid userId))
            {
                return Unauthorized();
            }

            command.UserId = userId;

            await _mediator.Send(command);
            return NoContent();
        }

        [Authorize]
        [HttpPost("profilePicture")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadProfilePicture([FromForm] UploadFileRequest request)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out Guid userId)) return Unauthorized();

            var file = request.File;

            if (file == null || file.Length == 0)
            {
                return BadRequest(new { error = "No file provided." });
            }

            const int MaxFileSize = 5 * 1024 * 1024;
            if (file.Length > MaxFileSize)
            {
                return BadRequest(new { error = "File size exceeds 5MB limit." });
            }

            var allowedTypes = new[] { "image/jpeg", "image/png" };
            if (!allowedTypes.Contains(file.ContentType))
            {
                return BadRequest(new { error = "Only JPEG and PNG images are allowed." });
            }
          

            using var fileStream = file.OpenReadStream();

            var command = new UploadProfilePictureCommand
            {
                UserId = userId,
                ProfilePictureStream = fileStream,
                ProfilePictureFileName = file.FileName,
                ProfilePictureContentType = file.ContentType
            };

            var newKey = await _mediator.Send(command);

            return Ok(new { profilePictureKey = newKey });
        }


        [HttpDelete("profilePicture")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> RemoveProfilePicture()
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out Guid userId)) return Unauthorized();

            var command = new RemoveProfilePictureCommand { UserId = userId };

            await _mediator.Send(command);

            return NoContent();
        }


        [HttpPut("password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out Guid userId)) return Unauthorized();

            command.UserId = userId;

            await _mediator.Send(command);

            return NoContent(); // 204 No Content
        }
    }
}
