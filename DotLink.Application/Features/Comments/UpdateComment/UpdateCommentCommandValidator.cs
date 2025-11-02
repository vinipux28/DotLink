using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotLink.Application.Features.Comments.UpdateComment
{
    public class UpdateCommentCommandValidator : AbstractValidator<UpdateCommentCommand>
    {
        public UpdateCommentCommandValidator()
        {
            RuleFor(x => x.CommentId).NotEmpty().WithMessage("CommentId is required.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.")
                .MinimumLength(1).WithMessage("Content has to have minimum length of 1 character.")
                .MaximumLength(2000).WithMessage("Content has to have maximum length of 2000 characters.");
        }
    }
}
