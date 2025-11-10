using FluentValidation;
using DotLink.Application.Repositories;
using System.Threading.Tasks;
using System.Threading;

namespace DotLink.Application.Features.Users.UpdateUserProfile
{
    public class UpdateUserProfileCommandValidator : AbstractValidator<UpdateUserProfileCommand>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserProfileCommandValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;


            When(x => !string.IsNullOrWhiteSpace(x.NewUsername), () =>
            {
                RuleFor(x => x.NewUsername)
                    .Length(3, 20).WithMessage("Username has to have minimum length of 1 character.")
                    .MustAsync(BeUniqueUsername).WithMessage("This username is already in use.");
            });

            RuleFor(x => x)
                .Must(x => !string.IsNullOrWhiteSpace(x.NewUsername) || x.NewBio != null)
                .WithMessage("At least one field must be provided for an update.");
        }

        private async Task<bool> BeUniqueUsername(UpdateUserProfileCommand command, string? newUsername, CancellationToken token)
        {
            if (string.IsNullOrWhiteSpace(newUsername)) return true;
            return await _userRepository.IsUsernameUniqueAsync(newUsername!);
        }
    }
}