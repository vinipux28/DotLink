using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using DotLink.Application.Repositories;

namespace DotLink.Application.Features.Users.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Unit>
    {
        private readonly IUserRepository _userRepository;

        public ChangePasswordCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId)
                        ?? throw new ApplicationException("User not found.");

            bool isOldPasswordCorrect = user.VerifyPassword(request.OldPassword);

            if (!isOldPasswordCorrect)
            {
                throw new UnauthorizedAccessException("Invalid old password.");
            }

            user.UpdatePassword(request.NewPassword);

            await _userRepository.UpdateAsync(user);

            return Unit.Value;
        }
    }
}