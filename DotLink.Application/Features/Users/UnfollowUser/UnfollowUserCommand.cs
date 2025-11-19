using MediatR;
using System;

namespace DotLink.Application.Features.Users.UnfollowUser
{
    public class UnfollowUserCommand : IRequest<Unit>
    {
        public Guid FollowerId { get; set; }
        public Guid FolloweeId { get; set; }
    }
}
