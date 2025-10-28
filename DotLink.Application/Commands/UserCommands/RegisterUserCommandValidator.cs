using FluentValidation;

namespace DotLink.Application.Commands.UserCommands
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Incorrect Email format.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password has to have minimum length of 8 characters.");

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.");
        }
    }
}