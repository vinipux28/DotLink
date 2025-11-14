using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using DotLink.Application.Repositories;
using DotLink.Application.Exceptions;

namespace DotLink.Application.Features.Users.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Unit>
    {
        private readonly IUserRepository _userRepository;

        public ResetPasswordCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user == null)
            {
                throw new ApplicationException("Invalid reset request."); // To-Do: Consider a more specific exception type
            }

            bool isTokenValid = !string.IsNullOrWhiteSpace(user.PasswordResetToken) &&
                                user.PasswordResetToken == request.Token &&
                                user.PasswordResetTokenExpiry.HasValue &&
                                user.PasswordResetTokenExpiry.Value > DateTime.UtcNow;

            if (!isTokenValid)
            {
                user.ClearPasswordResetToken();
                await _userRepository.UpdateAsync(user);

                throw new DotLinkUnauthorizedAccessException("Invalid or expired password reset token.");
            }

            user.UpdatePassword(request.NewPassword);

            user.ClearPasswordResetToken();

            await _userRepository.UpdateAsync(user);

            return Unit.Value;
        }
    }
}