using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using DotLink.Application.Repositories;
using DotLink.Application.Services;
using DotLink.Application.Exceptions;

namespace DotLink.Application.Features.Users.RemoveProfilePicture
{
    public class RemoveProfilePictureCommandHandler : IRequestHandler<RemoveProfilePictureCommand, Unit>
    {
        private readonly IUserRepository _userRepository;
        private readonly IFileStorageService _fileStorageService;

        public RemoveProfilePictureCommandHandler(IUserRepository userRepository, IFileStorageService fileStorageService)
        {
            _userRepository = userRepository;
            _fileStorageService = fileStorageService;
        }

        public async Task<Unit> Handle(RemoveProfilePictureCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId)
                        ?? throw new DotLinkNotFoundException("User", request.UserId);

            if (string.IsNullOrWhiteSpace(user.ProfilePictureKey))
            {
                return Unit.Value;
            }

            await _fileStorageService.DeleteFileAsync(user.ProfilePictureKey);

            user.UpdateProfilePictureKey(null);

            await _userRepository.UpdateAsync(user);

            return Unit.Value;
        }
    }
}