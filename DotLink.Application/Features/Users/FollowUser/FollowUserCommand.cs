using MediatR;
using System;

namespace DotLink.Application.Features.Users.FollowUser
{
    public class FollowUserCommand : IRequest<Unit>
    {
        public Guid FollowerId { get; set; }
        public Guid FolloweeId { get; set; }
    }
}
