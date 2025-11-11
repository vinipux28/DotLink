using MediatR;

namespace DotLink.Application.Features.Users.ResetPassword
{
    public class ResetPasswordCommand : IRequest<Unit>
    {
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty; // for fast searching
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}