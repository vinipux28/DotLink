using DotLink.Application.Exceptions;
using DotLink.Application.Repositories;
using DotLink.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DotLink.Application.Features.Comments.CreateComment
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
            if (postExists is null)
            {
                throw new DotLinkNotFoundException("Post", request.PostId);
            }

            if (request.ParentCommentId.HasValue)
            {
                var parentComment = await _commentRepository.GetByIdAsync(request.ParentCommentId.Value);
                if (parentComment is null)
                {
                    throw new DotLinkNotFoundException("Comment", request.ParentCommentId.Value);
                }

                if (parentComment.PostId != request.PostId)
                {
                    throw new Exception("Cannot reply to a comment that belongs to a different post."); // TO-Do: Create a specific exception for this case
                }
            }

            var newComment = new Comment(
                Guid.NewGuid(),
                request.Content,
                request.UserId,
                request.PostId,
                request.ParentCommentId
            );

            await _commentRepository.AddAsync(newComment);

            return newComment.Id;
        }
    }
}