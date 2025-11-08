using MediatR;
using System;
using System.IO;

namespace DotLink.Application.Features.Users.UploadProfilePicture
{
    public class UploadProfilePictureCommand : IRequest<string>
    {
        public Guid UserId { get; set; }
        public Stream ProfilePictureStream { get; set; } = default!;
        public string ProfilePictureFileName { get; set; } = default!;
        public string ProfilePictureContentType { get; set; } = default!;
    }
}