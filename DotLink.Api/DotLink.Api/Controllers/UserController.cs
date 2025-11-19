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
using System.Security.Claims;

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
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user == null ? NotFound() : Ok(_mapperService.MapToUserDTO(user));
        }

        [Authorize]
        [HttpGet("me")]
        [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCurrentUser()
        {
            if (!TryGetUserId(out var userId)) return Unauthorized();

            var user = await _userRepository.GetByIdAsync(userId);
            return user == null ? NotFound() : Ok(_mapperService.MapToUserDTO(user));
        }

        [Authorize]
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserProfileCommand command, CancellationToken cancellationToken)
        {
            if (!TryGetUserId(out var userId)) return Unauthorized();

            command.UserId = userId;
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [Authorize]
        [HttpPost("profilePicture")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UploadProfilePicture([FromForm] UploadFileRequest request, CancellationToken cancellationToken)
        {
            if (!TryGetUserId(out var userId)) return Unauthorized();

            var file = request.File;
            if (file == null || file.Length == 0)
                return BadRequest(new { error = "No file provided." });

            if (file.Length > 5 * 1024 * 1024)
                return BadRequest(new { error = "File size exceeds 5MB limit." });

            var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif" };
            if (!allowedTypes.Contains(file.ContentType))
                return BadRequest(new { error = "Only JPEG, PNG and GIF images are allowed." });

            using var fileStream = file.OpenReadStream();

            var command = new UploadProfilePictureCommand
            {
                UserId = userId,
                ProfilePictureStream = fileStream,
                ProfilePictureFileName = file.FileName,
                ProfilePictureContentType = file.ContentType
            };

            var newKey = await _mediator.Send(command, cancellationToken);
            return Ok(new { profilePictureKey = newKey });
        }

        [Authorize]
        [HttpDelete("profilePicture")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RemoveProfilePicture(CancellationToken cancellationToken)
        {
            if (!TryGetUserId(out var userId)) return Unauthorized();

            await _mediator.Send(new RemoveProfilePictureCommand { UserId = userId }, cancellationToken);
            return NoContent();
        }

        [Authorize]
        [HttpPost("{id:guid}/follow")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Follow(Guid id, CancellationToken cancellationToken)
        {
            if (!TryGetUserId(out var userId)) return Unauthorized();

            await _mediator.Send(new FollowUserCommand { FollowerId = userId, FolloweeId = id }, cancellationToken);
            return NoContent();
        }

        [Authorize]
        [HttpPost("{id:guid}/unfollow")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Unfollow(Guid id, CancellationToken cancellationToken)
        {
            if (!TryGetUserId(out var userId)) return Unauthorized();

            await _mediator.Send(new UnfollowUserCommand { FollowerId = userId, FolloweeId = id }, cancellationToken);
            return NoContent();
        }

        [HttpGet("{id:guid}/followers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFollowers(Guid id, CancellationToken cancellationToken)
        {
            var followers = await _mediator.Send(new GetFollowersQuery { UserId = id }, cancellationToken);
            return Ok(followers);
        }

        [HttpGet("{id:guid}/followings")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFollowings(Guid id, CancellationToken cancellationToken)
        {
            var followings = await _mediator.Send(new GetFollowingsQuery { UserId = id }, cancellationToken);
            return Ok(followings);
        }

        [Authorize]
        [HttpPut("password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command, CancellationToken cancellationToken)
        {
            if (!TryGetUserId(out var userId)) return Unauthorized();

            command.UserId = userId;
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        private bool TryGetUserId(out Guid userId)
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(claim, out userId);
        }
    }
}