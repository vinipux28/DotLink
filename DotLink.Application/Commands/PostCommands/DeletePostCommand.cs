using MediatR;
using System;

namespace DotLink.Application.Commands.PostCommands
{
    public class DeletePostCommand : IRequest<Unit>
    {
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
    }
}