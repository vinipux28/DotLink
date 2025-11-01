using FluentValidation;

namespace DotLink.Application.Commands.PostCommands
{
    public class UpdatePostCommandValidator : AbstractValidator<UpdatePostCommand>
    {
        public UpdatePostCommandValidator()
        {
            RuleFor(x => x.PostId)
                .NotEmpty().WithMessage("Post ID is required.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Post title is required.")
                .MinimumLength(1).WithMessage("Title has to have minimum length of 1 character.")
                .MaximumLength(255).WithMessage("Title has to have maximum length of 255 characters.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Post content is required.")
                .MinimumLength(1).WithMessage("Content has to have minimum length of 1 character.");
        }
    }
}