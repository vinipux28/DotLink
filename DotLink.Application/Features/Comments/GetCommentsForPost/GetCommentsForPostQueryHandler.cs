using DotLink.Application.DTOs;
using DotLink.Application.Repositories;
using DotLink.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DotLink.Application.Features.Comments.GetCommentsForPost
{
    public class GetCommentsForPostQueryHandler : IRequestHandler<GetCommentsForPostQuery, PaginatedResponse<CommentDTO>>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ILogger<GetCommentsForPostQueryHandler> _logger;

        public GetCommentsForPostQueryHandler(ICommentRepository commentRepository, ILogger<GetCommentsForPostQueryHandler> logger)
        {
            _commentRepository = commentRepository;
            _logger = logger;
        }

        public async Task<PaginatedResponse<CommentDTO>> Handle(GetCommentsForPostQuery request, CancellationToken cancellationToken)
        {
            var (comments, totalCount) = await _commentRepository.GetPaginatedByPostIdAsync(
                request.PostId,
                request.PageNumber,
                request.PageSize
            );

            var commentDTOs = comments.Select(c => new CommentDTO(c)).ToList();

            _logger.LogInformation("Retrieved {Count} comments (total {TotalCount}) for post {PostId} (page {Page}, size {Size})",
                commentDTOs.Count, totalCount, request.PostId, request.PageNumber, request.PageSize);

            return new PaginatedResponse<CommentDTO>(
                commentDTOs,
                totalCount,
                request.PageNumber,
                request.PageSize
            );
        }

    }
}