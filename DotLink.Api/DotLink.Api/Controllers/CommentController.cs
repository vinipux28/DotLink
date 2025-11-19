using DotLink.Application.Features.Comments.CreateComment;
using DotLink.Application.Features.Comments.DeleteComment;
using DotLink.Application.Features.Comments.GetCommentReplies;
using DotLink.Application.Features.Comments.UpdateComment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateComment(Guid postId, [FromBody] CreateCommentCommand command, CancellationToken cancellationToken)
        {
            if (!TryGetUserId(out var userId)) return Unauthorized();

            command.UserId = userId;
            command.PostId = postId;

            _logger.LogDebug("ParentCommentId is {ParentCommentId}", command.ParentCommentId);

            var commentId = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction("GetPostComments", "Post", new { postId }, new { CommentId = commentId });
        }

        [HttpPost("{commentId:guid}/reply")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ReplyToComment(Guid postId, Guid commentId, [FromBody] CreateCommentCommand command, CancellationToken cancellationToken)
        {
            if (!TryGetUserId(out var userId)) return Unauthorized();

            command.UserId = userId;
            command.PostId = postId;
            command.ParentCommentId = commentId;

            var replyCommentId = await _mediator.Send(command, cancellationToken);

            return CreatedAtAction("GetPostComments", "Post", new { postId }, new { CommentId = replyCommentId });
        }

        [HttpGet("{parentCommentId:guid}/replies")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCommentReplies(
            Guid parentCommentId,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default)
        {
            var query = new GetCommentRepliesQuery
            {
                ParentCommentId = parentCommentId,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPut("{commentId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> UpdateComment(Guid commentId, [FromBody] UpdateCommentCommand command, CancellationToken cancellationToken)
        {
            if (!TryGetUserId(out var userId)) return Unauthorized();

            command.CommentId = commentId;

            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{commentId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteComment(Guid commentId, CancellationToken cancellationToken)
        {
            if (!TryGetUserId(out var userId)) return Unauthorized();

            var command = new DeleteCommentCommand
            {
                CommentId = commentId
            };

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