using FluentValidation;

namespace DotLink.Application.Features.Users.GetFollowings
{
    public class GetFollowingsQueryValidator : AbstractValidator<GetFollowingsQuery>
    {
        public GetFollowingsQueryValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required.");
        }
    }
}
