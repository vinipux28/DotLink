using FluentValidation;

namespace DotLink.Application.Queries.PostQueries
{
    public class GetRecentPostsQueryValidator : AbstractValidator<GetRecentPostsQuery>
    {
        public GetRecentPostsQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1).WithMessage("Page number must not be less than 1.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 50).WithMessage("PageSize must be between 1 and 50.");
        }
    }
}