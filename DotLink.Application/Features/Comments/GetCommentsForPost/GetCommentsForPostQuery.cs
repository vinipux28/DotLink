using DotLink.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;

namespace DotLink.Application.Features.Comments.GetCommentsForPost
{
    public class GetCommentsForPostQuery : IRequest<List<CommentDTO>>
    {
        public Guid PostId { get; set; }
    }
}