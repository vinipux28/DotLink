using DotLink.Application.Features.Comments.CreateComment;
using DotLink.Application.Features.Comments.DeleteComment;
using DotLink.Application.Features.Comments.GetCommentReplies;
using DotLink.Application.Features.Comments.UpdateComment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace DotLink.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/post/{postId:guid}/comment")]
    public class CommentController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CommentController> _logger;
        public CommentController(IMediator mediator, ILogger<CommentController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(Guid postId, [FromBody] CreateCommentCommand command)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out Guid userId))
            {
                return Unauthorized();
            }

            command.UserId = userId;
            command.PostId = postId;

            _logger.LogDebug("ParentCommentId is {ParentCommentId}", command.ParentCommentId);

            Guid commentId = await _mediator.Send(command);
            // Return location of the post comments list (client can then retrieve or filter as needed)
            return CreatedAtAction("GetPostComments", "Post", new { postId = postId }, new { CommentId = commentId });
        }


        [HttpPost("{commentId:guid}/reply")]
        public async Task<IActionResult> ReplyToComment(Guid postId, Guid commentId, [FromBody] CreateCommentCommand command)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out Guid userId))
            {
                return Unauthorized();
            }
            command.UserId = userId;
            command.PostId = postId;
            command.ParentCommentId = commentId;

            Guid replyCommentId = await _mediator.Send(command);
            // Return location of the post comments list
            return CreatedAtAction("GetPostComments", "Post", new { postId = postId }, new { CommentId = replyCommentId });
        }

        [HttpGet("{parentCommentId:guid}/replies")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCommentReplies(
            Guid parentCommentId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetCommentRepliesQuery
            {
                ParentCommentId = parentCommentId,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query);

            return Ok(result);
        }


        [HttpPut("{commentId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateComment(Guid commentId, [FromBody] UpdateCommentCommand command)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ;
            if (!Guid.TryParse(userIdClaim, out Guid userId))
            {
                return Unauthorized();
            }
            
            command.CommentId = commentId;
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{commentId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteComment(Guid commentId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(userIdClaim, out Guid userId))
            {
                return Unauthorized();
            }
            var command = new DeleteCommentCommand
            {
                CommentId = commentId
            };

            await _mediator.Send(command);
            return NoContent();
        }
    }
}
