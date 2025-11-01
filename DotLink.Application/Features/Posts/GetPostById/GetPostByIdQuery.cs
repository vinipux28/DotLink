using DotLink.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using DotLink.Domain.Entities;

namespace DotLink.Application.Features.Posts.GetPostById
{
    public class GetPostByIdQuery : IRequest<PostDTO>
    {
        public Guid PostId { get; set; }
        public bool IncludeComments { get; set; } = false;

    }
}
