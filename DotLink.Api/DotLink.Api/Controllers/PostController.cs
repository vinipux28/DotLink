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
using DotLink.Application.Features.Comments.CreateComment;

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
        public async Task<IActionResult> GetPostById(Guid postId, [FromQuery] GetPostByIdQuery query)
        {
            query.PostId = postId;

            try
            {
                var post = await _mediator.Send(query);
                return Ok(post);
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
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

        [HttpGet("recent")]
        public async Task<IActionResult> GetRecentPosts([FromQuery] GetRecentPostsQuery query)
        {
            var posts = await _mediator.Send(query);

            return Ok(posts);
        }


        [Authorize]
        [HttpPost("vote/{postId:guid}")]
        public async Task<IActionResult> CastVote(Guid postId, [FromBody] CastVoteCommand command)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out Guid userId))
            {
                return Unauthorized();
            }

            command.UserId = userId;
            command.PostId = postId;

            try
            {
                await _mediator.Send(command);

                return Ok(new { Message = "Vote cast successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        [Authorize]
        [HttpPost("{postId:guid}/comment")]
        public async Task<IActionResult> CreateComment(Guid postId, [FromBody] CreateCommentCommand command)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out Guid userId))
            {
                return Unauthorized();
            }

            command.UserId = userId;
            command.PostId = postId;

            try
            {
                Guid commentId = await _mediator.Send(command);
                return CreatedAtAction(nameof(CreateComment), new { id = commentId }, new { CommentId = commentId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        [Authorize]
        [HttpPut("{postId:guid}")]
        public async Task<IActionResult> UpdatePost(Guid postId, [FromBody] UpdatePostCommand command)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out Guid userId)) return Unauthorized();

            command.PostId = postId;
            command.UserId = userId;

            try
            {
                await _mediator.Send(command);
                return NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("not found")) return NotFound(new { error = ex.Message });
                return BadRequest(new { error = ex.Message });
            }
        }

        [Authorize]
        [HttpDelete("{postId:guid}")]
        public async Task<IActionResult> DeletePost(Guid postId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out Guid userId)) return Unauthorized();

            var command = new DeletePostCommand
            {
                PostId = postId,
                UserId = userId
            };

            try
            {
                await _mediator.Send(command);
                return NoContent(); // HTTP 204 No Content
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid(); // HTTP 403 Forbidden
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}