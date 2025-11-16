using DotLink.Application.Exceptions;
using DotLink.Application.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotLink.Application.Features.Comments.DeleteComment
{
    public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, Unit>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ILogger<DeleteCommentCommandHandler> _logger;
        public DeleteCommentCommandHandler(ICommentRepository commentRepository, ILogger<DeleteCommentCommandHandler> logger)
        {
            _commentRepository = commentRepository;
            _logger = logger;
        }
        public async Task<Unit> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await _commentRepository.GetByIdAsync(request.CommentId);
            if (comment is null)
            {
                _logger.LogWarning("Attempt to delete non-existing comment {CommentId}", request.CommentId);
                throw new DotLinkNotFoundException("Comment", request.CommentId);
            }

            await _commentRepository.DeleteAsync(comment);

            _logger.LogInformation("Comment with ID: {CommentId} has been deleted.", request.CommentId);

            return Unit.Value;
        }
    }
}
