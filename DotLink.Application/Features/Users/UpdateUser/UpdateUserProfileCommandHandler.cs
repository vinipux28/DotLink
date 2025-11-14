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
                throw new DotLink.Application.Exceptions.DotLinkNotFoundException("User", request.UserId);
            }

            if (!string.IsNullOrWhiteSpace(request.NewUsername))
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

            return Unit.Value;
        }
    }
}