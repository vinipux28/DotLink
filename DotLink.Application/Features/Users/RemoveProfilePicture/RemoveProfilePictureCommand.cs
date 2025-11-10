using MediatR;
using System;

namespace DotLink.Application.Features.Users.RemoveProfilePicture
{
    public class RemoveProfilePictureCommand : IRequest<Unit>
    {
        public Guid UserId { get; set; }
    }
}