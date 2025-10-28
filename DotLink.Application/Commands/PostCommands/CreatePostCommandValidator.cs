using FluentValidation;

namespace DotLink.Application.Commands.PostCommands
{
    public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
    {
        public CreatePostCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(255).WithMessage("Title has to have maximum length of 255 characters.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Post content can't be blank.");
        }
    }
}