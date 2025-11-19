using DotLink.Api.Models;
using DotLink.Application.DTOs;
using DotLink.Application.Features.Users.ChangePassword;
using DotLink.Application.Features.Users.RemoveProfilePicture;
using DotLink.Application.Features.Users.UploadProfilePicture;
using DotLink.Application.Features.Users.FollowUser;
using DotLink.Application.Features.Users.UnfollowUser;
using DotLink.Application.Features.Users.GetFollowers;
using DotLink.Application.Features.Users.GetFollowings;
using DotLink.Application.Repositories;
using DotLink.Application.Services;
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
        private readonly IDTOMapperService _mapperService;
        public UserController(IMediator mediator, IUserRepository userRepository, IDTOMapperService mapperService)
        {
            _mediator = mediator;
            _userRepository = userRepository;
            _mapperService = mapperService;
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(_mapperService.MapToUserDTO(user));
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
            return Ok(_mapperService.MapToUserDTO(user));
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


        [Authorize]
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

        [Authorize]
        [HttpPost("{id:guid}/follow")]
        public async Task<IActionResult> Follow(Guid id)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out Guid userId)) return Unauthorized();

            var command = new FollowUserCommand
            {
                FollowerId = userId,
                FolloweeId = id
            };

            await _mediator.Send(command);

            return NoContent();
        }

        [Authorize]
        [HttpPost("{id:guid}/unfollow")]
        public async Task<IActionResult> Unfollow(Guid id)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out Guid userId)) return Unauthorized();

            var command = new UnfollowUserCommand
            {
                FollowerId = userId,
                FolloweeId = id
            };

            await _mediator.Send(command);

            return NoContent();
        }

        // Get followers of a user
        [HttpGet("{id:guid}/followers")]
        public async Task<IActionResult> GetFollowers(Guid id)
        {
            var query = new GetFollowersQuery { UserId = id };
            var followers = await _mediator.Send(query);
            return Ok(followers);
        }

        // Get followings of a user
        [HttpGet("{id:guid}/followings")]
        public async Task<IActionResult> GetFollowings(Guid id)
        {
            var query = new GetFollowingsQuery { UserId = id };
            var followings = await _mediator.Send(query);
            return Ok(followings);
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
