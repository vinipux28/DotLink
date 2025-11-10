using MediatR;
using System;

namespace DotLink.Application.Features.Users.ChangePassword
{
    public class ChangePasswordCommand : IRequest<Unit>
    {
        public Guid UserId { get; set; }

        public string OldPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}