using MediatR;
using System;

namespace DotLink.Application.Commands.PostCommands
{
    public class UpdatePostCommand : IRequest<Unit>
    {
        public Guid PostId { get; set; }

        public Guid UserId { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }
    }
}