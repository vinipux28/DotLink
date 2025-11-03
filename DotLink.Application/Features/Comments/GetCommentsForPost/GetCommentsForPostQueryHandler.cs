using DotLink.Application.DTOs;
using DotLink.Application.Repositories;
using DotLink.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DotLink.Application.Features.Comments.GetCommentsForPost
{
    public class GetCommentsForPostQueryHandler : IRequestHandler<GetCommentsForPostQuery, List<CommentDTO>>
    {
        private readonly ICommentRepository _commentRepository;

        public GetCommentsForPostQueryHandler(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<List<CommentDTO>> Handle(GetCommentsForPostQuery request, CancellationToken cancellationToken)
        {
            var comments = await _commentRepository.GetByPostIdAsync(request.PostId);

            comments.Select(c => new CommentDTO(c));
        }

    }
}