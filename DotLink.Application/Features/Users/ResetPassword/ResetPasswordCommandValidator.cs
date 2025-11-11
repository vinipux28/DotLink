using FluentValidation;

namespace DotLink.Application.Features.Users.ResetPassword
{
    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(x => x.Token)
                .NotEmpty()
                .WithMessage("Reset token is required.");

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .MinimumLength(8);

            RuleFor(x => x.ConfirmNewPassword)
                .Equal(x => x.NewPassword)
                .WithMessage("The new password and confirmation password do not match.");
        }
    }
}