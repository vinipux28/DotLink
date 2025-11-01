using FluentValidation;

namespace DotLink.Application.Features.Posts.CreatePost
{
    public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
    {
        public CreatePostCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MinimumLength(1).WithMessage("Title has to have minimum length of 1 character.")
                .MaximumLength(255).WithMessage("Title has to have maximum length of 255 characters.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Post content can't be blank.")
                .MinimumLength(1).WithMessage("Content has to have minimum length of 1 character.");
        }
    }
}