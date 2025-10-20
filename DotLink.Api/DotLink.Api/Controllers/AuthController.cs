using Microsoft.AspNetCore.Mvc;
using MediatR;
using DotLink.Application.Commands.UserCommands;
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

            try
            {
                Guid userId = await _mediator.Send(command);

                return CreatedAtAction(nameof(Register), new { id = userId }, new { UserId = userId });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
        }

    }
}