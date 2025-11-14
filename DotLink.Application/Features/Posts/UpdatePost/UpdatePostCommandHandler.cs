using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLink.Application.Repositories;
using DotLink.Application.Exceptions;

namespace DotLink.Application.Features.Posts.UpdatePost
{
    public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, Unit>
    {
        private readonly IPostRepository _postRepository;
        public UpdatePostCommandHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<Unit> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _postRepository.GetByIdAsync(request.PostId);

            if (post is null)
            {
                throw new DotLinkNotFoundException("Post", request.PostId);
            }

            if (post.AuthorId != request.UserId)
            {
                throw new DotLinkUnauthorizedAccessException("Only author is allowed to edit this post.");
            }

            post.UpdateTitle(request.Title);
            post.UpdateContent(request.Content);

            await _postRepository.UpdateAsync(post);

            return Unit.Value;
        }
    }
}
