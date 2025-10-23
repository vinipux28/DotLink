using MediatR;
using System;

namespace DotLink.Application.Commands.PostCommands
{
    public class CreatePostCommand : IRequest<Guid>
    {
        public Guid UserId { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }
    }
}