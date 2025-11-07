using FluentValidation;

namespace DotLink.Application.Features.Comments.GetCommentsForPost
{
    public class GetCommentsForPostQueryValidator : AbstractValidator<GetCommentsForPostQuery>
    {
        public GetCommentsForPostQueryValidator()
        {
            RuleFor(x => x.PostId)
                .NotEmpty().WithMessage("Post ID is required.");

            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1).WithMessage("Page number has be greater or equal to 1.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 50).WithMessage("Page size has to be in range betweeen 1 and 50 (inclusive).");
        }
    }
}