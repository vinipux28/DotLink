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
        private readonly IUserRepository _userRepository;
        private readonly ILogger<DeleteCommentCommandHandler> _logger;
        public DeleteCommentCommandHandler(ICommentRepository commentRepository, ILogger<DeleteCommentCommandHandler> logger, IUserRepository userRepository)
        {
            _commentRepository = commentRepository;
            _logger = logger;
            _userRepository = userRepository;
        }
        public async Task<Unit> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await _commentRepository.GetByIdAsync(request.CommentId);
            if (comment is null)
            {
                _logger.LogWarning("Attempt to delete non-existing comment {CommentId}", request.CommentId);
                throw new DotLinkNotFoundException("Comment", request.CommentId);
            }

            var user = await _userRepository.GetByIdAsync(request.UserId);

            if (user is null) {
                _logger.LogWarning("User with ID: {UserId} not found when attempting to delete comment.", request.UserId);
                throw new DotLinkUnauthorizedAccessException("User not found.");
            }

            if (comment.AuthorId == user.Id)
            {
                _logger.LogWarning("User with ID: {UserId} attempted to delete comment with ID: {CommentId} without permission.", request.UserId, request.CommentId);
                throw new DotLinkUnauthorizedAccessException("You do not have permission to delete this comment.");
            }

            await _commentRepository.DeleteAsync(comment);

            _logger.LogInformation("Comment with ID: {CommentId} has been deleted.", request.CommentId);

            return Unit.Value;
        }
    }
}
