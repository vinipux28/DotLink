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
    public class GetCommentsForPostQueryHandler : IRequestHandler<GetCommentsForPostQuery, PaginatedResponse<CommentDTO>>
    {
        private readonly ICommentRepository _commentRepository;

        public GetCommentsForPostQueryHandler(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<PaginatedResponse<CommentDTO>> Handle(GetCommentsForPostQuery request, CancellationToken cancellationToken)
        {
            var (comments, totalCount) = await _commentRepository.GetPaginatedByPostIdAsync(
                request.PostId,
                request.PageNumber,
                request.PageSize
            );

            var commentDTOs = comments.Select(c => new CommentDTO(c)).ToList();

            return new PaginatedResponse<CommentDTO>(
                commentDTOs,
                totalCount,
                request.PageNumber,
                request.PageSize
            );
        }

    }
}