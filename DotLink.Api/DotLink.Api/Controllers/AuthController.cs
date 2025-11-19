using DotLink.Application.Features.Users.ForgotPassword;
using DotLink.Application.Features.Users.LoginUser;
using DotLink.Application.Features.Users.RegisterUser;
using DotLink.Application.Features.Users.ResetPassword;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DotLink.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Guid userId = await _mediator.Send(command);

            // Return location of the newly created user resource (GET api/user/{id})
            return CreatedAtAction("GetUserById", "User", new { id = userId }, new { UserId = userId });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string token = await _mediator.Send(command);

            return Ok(new { token = token });
        }


        [HttpPost("forgot-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command)
        {
            await _mediator.Send(command);

            return Ok(new { Message = "If account exists, a password reset link has been sent to the email." });
        }


        [HttpPost("reset-password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
        {
            await _mediator.Send(command);

            return NoContent(); // 204 No Content
        }

    }
}