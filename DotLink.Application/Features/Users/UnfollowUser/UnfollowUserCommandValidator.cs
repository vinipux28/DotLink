using FluentValidation;

namespace DotLink.Application.Features.Users.UnfollowUser
{
    public class UnfollowUserCommandValidator : AbstractValidator<UnfollowUserCommand>
    {
        public UnfollowUserCommandValidator()
        {
            RuleFor(x => x.FollowerId).NotEmpty().WithMessage("FollowerId is required.");
            RuleFor(x => x.FolloweeId).NotEmpty().WithMessage("FolloweeId is required.");

            RuleFor(x => x).Must(cmd => cmd.FollowerId != cmd.FolloweeId)
                .WithMessage("A user cannot unfollow themselves.");
        }
    }
}
