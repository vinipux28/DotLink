using MediatR;
using System;
using System.Collections.Generic;

namespace DotLink.Application.Features.Users.GetFollowings
{
    public class GetFollowingsQuery : IRequest<List<Guid>>
    {
        public Guid UserId { get; set; }
    }
}
