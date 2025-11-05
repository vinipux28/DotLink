using DotLink.Application.Repositories;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotLink.Application.DTOs;
using System.Collections.Generic;

namespace DotLink.Application.Features.Comments.GetCommentReplies
{
    public class GetCommentRepliesQueryHandler : IRequestHandler<GetCommentRepliesQuery, PaginatedResponse<CommentDTO>>
    {
        private readonly ICommentRepository _commentRepository;

        public GetCommentRepliesQueryHandler(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<PaginatedResponse<CommentDTO>> Handle(GetCommentRepliesQuery request, CancellationToken cancellationToken)
        {
            var (comments, totalCount) = await _commentRepository.GetPaginatedRepliesAsync(
                request.ParentCommentId,
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