using DotLink.Application.Exceptions;
using DotLink.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DotLink.Application.Features.Comments.UpdateComment
{
    public class UpdateCommentCommandHandler
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ILogger<UpdateCommentCommandHandler> _logger;
        public UpdateCommentCommandHandler(ICommentRepository commentRepository, ILogger<UpdateCommentCommandHandler> logger)
        {
            _commentRepository = commentRepository;
            _logger = logger;
        }

        public async Task Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await _commentRepository.GetByIdAsync(request.CommentId);
            if (comment is null)
            {
                _logger.LogWarning("Attempt to update non-existing comment {CommentId}", request.CommentId);
                throw new DotLinkNotFoundException("Comment", request.CommentId);
            }
            comment.UpdateContent(request.Content);
            await _commentRepository.UpdateAsync(comment);

            _logger.LogInformation("Comment {CommentId} updated", request.CommentId);
        }

    }
}
