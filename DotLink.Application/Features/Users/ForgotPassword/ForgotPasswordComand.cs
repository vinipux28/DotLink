using MediatR;
using System.ComponentModel.DataAnnotations;

namespace DotLink.Application.Features.Users.ForgotPassword
{
    public class ForgotPasswordCommand : IRequest<Unit>
    {
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}