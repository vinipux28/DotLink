using FluentValidation;

namespace DotLink.Application.Features.Users.GetFollowers
{
    public class GetFollowersQueryValidator : AbstractValidator<GetFollowersQuery>
    {
        public GetFollowersQueryValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required.");
        }
    }
}
