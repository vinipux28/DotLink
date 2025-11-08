using DotLink.Application.DTOs;
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
        [HttpPut]
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
    }
}
