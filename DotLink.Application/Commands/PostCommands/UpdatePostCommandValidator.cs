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
                .MaximumLength(255).WithMessage("Title has to have maximum length of 22 characters.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Post content is required.");
        }
    }
}