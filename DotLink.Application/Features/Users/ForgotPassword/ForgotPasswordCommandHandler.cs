using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using DotLink.Application.Repositories;
using DotLink.Application.Services;

namespace DotLink.Application.Features.Users.ForgotPassword
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Unit>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public ForgotPasswordCommandHandler(IUserRepository userRepository, IEmailService emailService)
        {
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public async Task<Unit> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user == null)
            {
                await Task.Delay(500, cancellationToken); // mock delay
                return Unit.Value;
            }

            string token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            DateTime expiry = DateTime.UtcNow.AddHours(1);

            user.SetPasswordResetToken(token, expiry);
            await _userRepository.UpdateAsync(user);

            string resetLink = $"https://TO-DO/reset-password?token={token}&email={user.Email}";

            await _emailService.SendPasswordResetEmailAsync(user.Email, user.Username, resetLink);

            return Unit.Value;
        }
    }
}