using DotLink.Application.Features.Comments.CreateComment;
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
        public CommentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Authorize]
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

            Console.WriteLine($"ParentCommentId is {command.ParentCommentId}");
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
            try
            {
                Guid replyCommentId = await _mediator.Send(command);
                return CreatedAtAction(nameof(ReplyToComment), new { id = replyCommentId }, new { CommentId = replyCommentId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


    }
}
