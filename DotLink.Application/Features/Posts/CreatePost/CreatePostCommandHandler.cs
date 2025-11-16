using DotLink.Application.Repositories;
using DotLink.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DotLink.Application.Features.Posts.CreatePost
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Guid>
    {
        private readonly IPostRepository _postRepository;
        private readonly ILogger<CreatePostCommandHandler> _logger;

        public CreatePostCommandHandler(IPostRepository postRepository, ILogger<CreatePostCommandHandler> logger)
        {
            _postRepository = postRepository;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var newPost = new Post(
                Guid.NewGuid(),
                request.UserId,
                request.Title,
                request.Content
            );

            await _postRepository.AddAsync(newPost);

            _logger.LogInformation("Post {PostId} created by User {UserId}", newPost.Id, request.UserId);

            return newPost.Id;
        }
    }
}