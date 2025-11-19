using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using DotLink.Application.Features.Posts.GetPostById;
using DotLink.Application.Features.Posts.CreatePost;
using DotLink.Application.Features.Posts.GetRecentPosts;
using DotLink.Application.Features.Posts.CastVote;
using DotLink.Application.Features.Posts.UpdatePost;
using DotLink.Application.Features.Posts.DeletePost;
using DotLink.Application.Features.Comments.GetCommentsForPost;

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

        [HttpGet("{postId:guid}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPostById(Guid postId, [FromQuery] GetPostByIdQuery query, CancellationToken cancellationToken)
        {
            query.PostId = postId;
            var post = await _mediator.Send(query, cancellationToken);
            return Ok(post);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostCommand command, CancellationToken cancellationToken)
        {
            if (!TryGetUserId(out var userId)) return Unauthorized();

            command.UserId = userId;
            var postId = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction(nameof(GetPostById), new { postId }, new { PostId = postId });
        }

        [AllowAnonymous]
        [HttpGet("recent")]
        [ProducesResponseType(typeof(IEnumerable<object>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetRecentPosts([FromQuery] GetRecentPostsQuery query, CancellationToken cancellationToken)
        {
            var posts = await _mediator.Send(query, cancellationToken);
            return Ok(posts);
        }

        [HttpPost("vote/{postId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CastVote(Guid postId, [FromBody] CastVoteCommand command, CancellationToken cancellationToken)
        {
            if (!TryGetUserId(out var userId)) return Unauthorized();

            command.UserId = userId;
            command.PostId = postId;

            await _mediator.Send(command, cancellationToken);
            return Ok(new { Message = "Vote cast successfully." });
        }

        [HttpPut("{postId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdatePost(Guid postId, [FromBody] UpdatePostCommand command, CancellationToken cancellationToken)
        {
            if (!TryGetUserId(out var userId)) return Unauthorized();

            command.PostId = postId;
            command.UserId = userId;

            try
            {
                await _mediator.Send(command, cancellationToken);
                return NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        [HttpDelete("{postId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeletePost(Guid postId, CancellationToken cancellationToken)
        {
            if (!TryGetUserId(out var userId)) return Unauthorized();

            var command = new DeletePostCommand
            {
                PostId = postId,
                UserId = userId
            };

            try
            {
                await _mediator.Send(command, cancellationToken);
                return NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        [HttpGet("{postId:guid}/comments")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPostComments(Guid postId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default)
        {
            var query = new GetCommentsForPostQuery
            {
                PostId = postId,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        private bool TryGetUserId(out Guid userId)
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(claim, out userId);
        }
    }
}