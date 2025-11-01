using MediatR;
using System;

namespace DotLink.Application.Features.Posts.CastVote
{
    public class CastVoteCommand : IRequest<Unit>
    {
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        public bool? IsUpvote { get; set; }
    }
}