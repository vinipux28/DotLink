using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using DotLink.Application.Repositories;
using DotLink.Application.Services;

namespace DotLink.Application.Features.Users.UploadProfilePicture
{
    public class UploadProfilePictureCommandHandler : IRequestHandler<UploadProfilePictureCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IFileStorageService _fileStorageService;

        public UploadProfilePictureCommandHandler(IUserRepository userRepository, IFileStorageService fileStorageService)
        {
            _userRepository = userRepository;
            _fileStorageService = fileStorageService;
        }

        public async Task<string> Handle(UploadProfilePictureCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId)
                       ?? throw new Exception("User not found.");

            if (!string.IsNullOrWhiteSpace(user.ProfilePictureKey))
            {
                await _fileStorageService.DeleteFileAsync(user.ProfilePictureKey);
            }

            var newKey = await _fileStorageService.UploadFileAsync(
                request.ProfilePictureStream,
                request.ProfilePictureFileName,
                request.ProfilePictureContentType,
                "profile-pics"
            );

            // 3. Обновление сущности новым ключом
            user.UpdateProfilePictureKey(newKey);

            await _userRepository.UpdateAsync(user);

            return newKey;
        }
    }
}