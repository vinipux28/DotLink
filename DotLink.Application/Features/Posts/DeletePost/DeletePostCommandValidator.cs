using FluentValidation;
using System;

namespace DotLink.Application.Commands.PostCommands
{
    public class DeletePostCommandValidator : AbstractValidator<DeletePostCommand>
    {
        public DeletePostCommandValidator()
        {
            RuleFor(x => x.PostId)
                .NotEmpty().WithMessage("Post ID is required.");
        }
    }
}