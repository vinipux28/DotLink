using DotLink.Application.Repositories;
using DotLink.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DotLink.Application.Commands.CommentCommands
{
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, Guid>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;

        public CreateCommentCommandHandler(ICommentRepository commentRepository, IPostRepository postRepository)
        {
            _commentRepository = commentRepository;
            _postRepository = postRepository;
        }

        public async Task<Guid> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var postExists = await _postRepository.GetByIdAsync(request.PostId);
            if (postExists == null)
            {
                throw new Exception($"Post with ID {request.PostId} not found.");
            }

            var newComment = new Comment(
                Guid.NewGuid(),
                request.PostId,
                request.UserId,
                request.Content
            );

            await _commentRepository.AddAsync(newComment);

            return newComment.Id;
        }
    }
}