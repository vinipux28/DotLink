using MediatR;
using System;
using DotLink.Application.DTOs;

namespace DotLink.Application.Features.Comments.GetCommentReplies
{
    public class GetCommentRepliesQuery : IRequest<PaginatedResponse<CommentDTO>>
    {
        public Guid ParentCommentId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}