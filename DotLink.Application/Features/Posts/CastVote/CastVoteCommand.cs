using MediatR;
using System;

namespace DotLink.Application.Commands.PostCommands
{
    public class CastVoteCommand : IRequest<Unit>
    {
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        public bool? IsUpvote { get; set; }
    }
}