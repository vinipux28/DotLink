using FluentValidation;
using System;

namespace DotLink.Application.Features.Comments.CreateComment
{
    public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
    {
        public CreateCommentCommandValidator()
        {
            RuleFor(x => x.PostId)
                .NotEmpty().WithMessage("PostId is required.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content can not be blank.")
                .MinimumLength(1).WithMessage("Content has to have minimum length of 1 character.")
                .MaximumLength(2000).WithMessage("Content has to have maximum length of 2000 characters.");
        }
    }
}