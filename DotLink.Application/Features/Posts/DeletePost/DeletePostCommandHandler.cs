using DotLink.Application.Exceptions;
using DotLink.Application.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DotLink.Application.Features.Posts.DeletePost
{
    public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, Unit>
    {
        private readonly IPostRepository _postRepository;

        public DeletePostCommandHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<Unit> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _postRepository.GetByIdAsync(request.PostId);

            if (post is null)
            {
                return Unit.Value;
            }

            if (post.AuthorId != request.UserId)
            {
                throw new DotLinkUnauthorizedAccessException("Only author is allowed to delete this post.");
            }

            await _postRepository.DeleteAsync(post);

            return Unit.Value;
        }
    }
}