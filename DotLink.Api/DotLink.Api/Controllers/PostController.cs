using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using DotLink.Application.Queries.PostQueries;
using DotLink.Application.Commands.PostCommands;

namespace DotLink.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PostController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetPostById(Guid id)
        {
            // This method is a placeholder for CreatedAtAction to reference.
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostCommand command)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null || !Guid.TryParse(userIdClaim, out Guid userId))
            {
                return Unauthorized();
            }

            command.UserId = userId;

            Guid postId = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetPostById), new { id = postId }, new { PostId = postId });
        }

        [HttpGet("/recent")]
        public async Task<IActionResult> GetRecentPosts([FromQuery] GetRecentPostsQuery query)
        {
            var posts = await _mediator.Send(query);

            return Ok(posts);
        }
    }
}