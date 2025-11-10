using FluentValidation;

namespace DotLink.Application.Features.Users.ChangePassword
{
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(x => x.OldPassword)
                .NotEmpty()
                .WithMessage("Old password is required.");

            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .MinimumLength(8)
                .WithMessage("New password has to have minimum length of 8 characters.");

            RuleFor(x => x.NewPassword)
                .NotEqual(x => x.OldPassword)
                .WithMessage("New password cannot be the same as the old password.");

            RuleFor(x => x.ConfirmNewPassword)
                .Equal(x => x.NewPassword)
                .WithMessage("The new password and confirmation password do not match.");
        }
    }
}