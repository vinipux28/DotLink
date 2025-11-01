using FluentValidation;
using System;

namespace DotLink.Application.Features.Posts.GetPostById
{
    public class GetPostByIdQueryValidator : AbstractValidator<GetPostByIdQuery>
    {
        public GetPostByIdQueryValidator()
        {
            RuleFor(x => x.PostId)
                .NotEmpty().WithMessage("PostId is required.");
        }
    }
}