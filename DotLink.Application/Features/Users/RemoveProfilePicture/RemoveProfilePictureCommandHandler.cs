using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using DotLink.Application.Repositories;
using DotLink.Application.Services;
using DotLink.Application.Exceptions;
using Microsoft.Extensions.Logging;

namespace DotLink.Application.Features.Users.RemoveProfilePicture
{
    public class RemoveProfilePictureCommandHandler : IRequestHandler<RemoveProfilePictureCommand, Unit>
    {
        private readonly IUserRepository _userRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly ILogger<RemoveProfilePictureCommandHandler> _logger;

        public RemoveProfilePictureCommandHandler(IUserRepository userRepository, IFileStorageService fileStorageService, ILogger<RemoveProfilePictureCommandHandler> logger)
        {
            _userRepository = userRepository;
            _fileStorageService = fileStorageService;
            _logger = logger;
        }

        public async Task<Unit> Handle(RemoveProfilePictureCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId)
                        ?? throw new DotLinkNotFoundException("User", request.UserId);

            if (string.IsNullOrWhiteSpace(user.ProfilePictureKey))
            {
                _logger.LogInformation("No profile picture to remove for user {UserId}", user.Id);
                return Unit.Value;
            }

            await _fileStorageService.DeleteFileAsync(user.ProfilePictureKey);

            user.UpdateProfilePictureKey(null);

            await _userRepository.UpdateAsync(user);

            _logger.LogInformation("Removed profile picture for user {UserId}", user.Id);

            return Unit.Value;
        }
    }
}