using DotLink.Application.Repositories;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotLink.Application.DTOs;
using System.Collections.Generic;
using DotLink.Application.Services;

namespace DotLink.Application.Features.Comments.GetCommentReplies
{
    public class GetCommentRepliesQueryHandler : IRequestHandler<GetCommentRepliesQuery, PaginatedResponse<CommentDTO>>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IDTOMapperService _mapperService;

        public GetCommentRepliesQueryHandler(ICommentRepository commentRepository, IDTOMapperService mapperService)
        {
            _commentRepository = commentRepository;
            _mapperService = mapperService;
        }

        public async Task<PaginatedResponse<CommentDTO>> Handle(GetCommentRepliesQuery request, CancellationToken cancellationToken)
        {
            var (comments, totalCount) = await _commentRepository.GetPaginatedRepliesAsync(
                request.ParentCommentId,
                request.PageNumber,
                request.PageSize
            );

            var commentDTOs = comments.Select(c => _mapperService.MapToCommentDTO(c)).ToList();

            return new PaginatedResponse<CommentDTO>(
                commentDTOs,
                totalCount,
                request.PageNumber,
                request.PageSize
            );
        }
    }
}