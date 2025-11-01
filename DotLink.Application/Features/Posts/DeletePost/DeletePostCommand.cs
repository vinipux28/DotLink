using MediatR;
using System;

namespace DotLink.Application.Features.Posts.DeletePost
{
    public class DeletePostCommand : IRequest<Unit>
    {
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
    }
}