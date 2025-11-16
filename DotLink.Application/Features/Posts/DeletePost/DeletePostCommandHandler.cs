using DotLink.Application.Exceptions;
using DotLink.Application.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DotLink.Application.Features.Posts.DeletePost
{
    public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, Unit>
    {
        private readonly IPostRepository _postRepository;
        private readonly ILogger<DeletePostCommandHandler> _logger;

        public DeletePostCommandHandler(IPostRepository postRepository, ILogger<DeletePostCommandHandler> logger)
        {
            _postRepository = postRepository;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _postRepository.GetByIdAsync(request.PostId);

            if (post is null)
            {
                _logger.LogInformation("Delete requested for non-existing post {PostId}", request.PostId);
                return Unit.Value;
            }

            if (post.AuthorId != request.UserId)
            {
                _logger.LogWarning("Unauthorized delete attempt for post {PostId} by user {UserId}", request.PostId, request.UserId);
                throw new DotLinkUnauthorizedAccessException("Only author is allowed to delete this post.");
            }

            await _postRepository.DeleteAsync(post);

            _logger.LogInformation("Post {PostId} deleted by user {UserId}", request.PostId, request.UserId);

            return Unit.Value;
        }
    }
}