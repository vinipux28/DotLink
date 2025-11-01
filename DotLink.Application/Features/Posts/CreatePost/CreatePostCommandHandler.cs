using DotLink.Application.Repositories;
using DotLink.Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DotLink.Application.Commands.PostCommands
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Guid>
    {
        private readonly IPostRepository _postRepository;

        public CreatePostCommandHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
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

            return newPost.Id;
        }
    }
}