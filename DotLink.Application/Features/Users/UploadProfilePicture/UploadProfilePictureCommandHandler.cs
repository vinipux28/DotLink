using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using DotLink.Application.Repositories;
using DotLink.Application.Services;
using DotLink.Application.Exceptions;
using Microsoft.Extensions.Logging;

namespace DotLink.Application.Features.Users.UploadProfilePicture
{
    public class UploadProfilePictureCommandHandler : IRequestHandler<UploadProfilePictureCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly ILogger<UploadProfilePictureCommandHandler> _logger;

        public UploadProfilePictureCommandHandler(IUserRepository userRepository, IFileStorageService fileStorageService, ILogger<UploadProfilePictureCommandHandler> logger)
        {
            _userRepository = userRepository;
            _fileStorageService = fileStorageService;
            _logger = logger;
        }

        public async Task<string> Handle(UploadProfilePictureCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId)
                       ?? throw new DotLinkNotFoundException("User", request.UserId);

            if (!string.IsNullOrWhiteSpace(user.ProfilePictureKey))
            {
                _logger.LogInformation("Deleting existing profile picture {OldKey} for user {UserId}", user.ProfilePictureKey, user.Id);
                await _fileStorageService.DeleteFileAsync(user.ProfilePictureKey);
            }

            var newKey = await _fileStorageService.UploadFileAsync(
                request.ProfilePictureStream,
                request.ProfilePictureFileName,
                request.ProfilePictureContentType,
                "profile-pics"
            );

            user.UpdateProfilePictureKey(newKey);

            await _userRepository.UpdateAsync(user);

            _logger.LogInformation("User {UserId} uploaded new profile picture {Key}", user.Id, newKey);

            return newKey;
        }
    }
}