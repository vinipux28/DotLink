using DotLink.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;

namespace DotLink.Application.Features.Comments.GetCommentsForPost
{
    public class GetCommentsForPostQuery : IRequest<PaginatedResponse<CommentDTO>>
    {
        public Guid PostId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}