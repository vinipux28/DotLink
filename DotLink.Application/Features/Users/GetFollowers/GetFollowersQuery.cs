using MediatR;
using System;
using System.Collections.Generic;

namespace DotLink.Application.Features.Users.GetFollowers
{
    public class GetFollowersQuery : IRequest<List<Guid>>
    {
        public Guid UserId { get; set; }
    }
}
