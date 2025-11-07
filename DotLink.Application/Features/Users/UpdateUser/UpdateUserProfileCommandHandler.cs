using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using DotLink.Application.Repositories;
using DotLink.Application.Services;

namespace DotLink.Application.Features.Users.UpdateUserProfile
{
    public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, Unit>
    {
        private readonly IUserRepository _userRepository;
        private readonly IFileStorageService _fileStorageService;

        public UpdateUserProfileCommandHandler(IUserRepository userRepository, IFileStorageService fileStorageService)
        {
            _userRepository = userRepository;
            _fileStorageService = fileStorageService;
        }

        public async Task<Unit> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);

            if (user is null)
            {
                throw new Exception("User not found.");
            }

            string? newPictureUrl = null;


            if (request.ProfilePictureStream is not null && request.ProfilePictureFileName is not null)
            {
                if (!string.IsNullOrWhiteSpace(user.ProfilePictureUrl))
                {
                    await _fileStorageService.DeleteFileAsync(user.ProfilePictureUrl);
                }

                newPictureUrl = await _fileStorageService.UploadFileAsync(
                    request.ProfilePictureStream,
                    request.ProfilePictureFileName,
                    request.ProfilePictureContentType ?? "application/octet-stream",
                    "profile-pics"
                );
            }
            else if (request.RemoveProfilePicture && !string.IsNullOrWhiteSpace(user.ProfilePictureUrl))
            {
                await _fileStorageService.DeleteFileAsync(user.ProfilePictureUrl);
                newPictureUrl = null;
            }


            if (!string.IsNullOrWhiteSpace(request.NewUsername))
            {
                user.UpdateUsername(request.NewUsername);
            }

            if (request.NewBio != null)
            {
                user.UpdateBio(request.NewBio);
            }

            if (newPictureUrl != null || request.RemoveProfilePicture)
            {
                user.UpdateProfilePictureUrl(newPictureUrl);
            }

            await _userRepository.UpdateAsync(user);

            return Unit.Value;
        }
    }
}