using FluentValidation;
using System;

namespace DotLink.Application.Commands.CommentCommands
{
    public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
    {
        public CreateCommentCommandValidator()
        {
            RuleFor(x => x.PostId)
                .NotEmpty().WithMessage("PostId is required.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content can not be blank.")
                .MaximumLength(2000).WithMessage("Content has to have maximum length of 2000 characters.");
        }
    }
}