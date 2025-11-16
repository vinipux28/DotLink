using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using DotLink.Application.Repositories;
using DotLink.Application.Services;
using Microsoft.Extensions.Logging;

namespace DotLink.Application.Features.Users.UpdateUserProfile
{
    public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, Unit>
    {
        private readonly IUserRepository _userRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly ILogger<UpdateUserProfileCommandHandler> _logger;

        public UpdateUserProfileCommandHandler(IUserRepository userRepository, IFileStorageService fileStorageService, ILogger<UpdateUserProfileCommandHandler> logger)
        {
            _userRepository = userRepository;
            _fileStorageService = fileStorageService;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);

            if (user is null)
            {
                _logger.LogWarning("Attempt to update non-existing user {UserId}", request.UserId);
                throw new DotLink.Application.Exceptions.DotLinkNotFoundException("User", request.UserId);
            }

            var usernameChanged = !string.IsNullOrWhiteSpace(request.NewUsername) && request.NewUsername != user.Username;
            var bioChanged = request.NewBio != null && request.NewBio != user.Bio;
            var nameChanged = !string.IsNullOrWhiteSpace(request.newFirstName) && !string.IsNullOrWhiteSpace(request.newLastName) &&
                              (request.newFirstName != user.FirstName || request.newLastName != user.LastName);

            if (usernameChanged)
            {
                user.UpdateUsername(request.NewUsername);
            }

            if (request.NewBio != null)
            {
                user.UpdateBio(request.NewBio);
            }
            if (!string.IsNullOrWhiteSpace(request.newFirstName) && !string.IsNullOrWhiteSpace(request.newLastName))
            {
                user.UpdateName(request.newFirstName, request.newLastName);
            }

            await _userRepository.UpdateAsync(user);

            _logger.LogInformation("User {UserId} profile updated; usernameChanged={UsernameChanged}, bioChanged={BioChanged}, nameChanged={NameChanged}",
                request.UserId, usernameChanged, bioChanged, nameChanged);

            return Unit.Value;
        }
    }
}