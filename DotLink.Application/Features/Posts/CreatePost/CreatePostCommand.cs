using MediatR;
using System;

namespace DotLink.Application.Features.Posts.CreatePost
{
    public class CreatePostCommand : IRequest<Guid>
    {
        public Guid UserId { get; set; }

        public string Title { get; set; }
        public string Content { get; set; }
    }
}