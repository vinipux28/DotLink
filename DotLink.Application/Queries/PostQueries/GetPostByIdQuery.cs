using DotLink.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using DotLink.Domain.Entities;

namespace DotLink.Application.Queries.PostQueries
{
    public class GetPostByIdQuery : IRequest<PostDTO>
    {
        public Guid PostId { get; set; }
        public bool IncludeComments { get; set; } = false;

    }
}
