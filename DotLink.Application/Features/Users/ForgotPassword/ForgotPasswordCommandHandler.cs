using DotLink.Application.Configuration;
using DotLink.Application.Repositories;
using DotLink.Application.Services;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DotLink.Application.Features.Users.ForgotPassword
{
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Unit>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly ClientSettings _clientSettings;

        public ForgotPasswordCommandHandler(
            IUserRepository userRepository, 
            IEmailService emailService,
            IOptions<ClientSettings> clientSettings)
        {
            _userRepository = userRepository;
            _emailService = emailService;
            _clientSettings = clientSettings.Value;
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

            string baseUrl = _clientSettings.BaseUrl.TrimEnd('/');

            string resetLink = $"https://{baseUrl}/reset-password?token={token}&email={user.Email}";

            await _emailService.SendPasswordResetEmailAsync(user.Email, user.Username, resetLink);

            return Unit.Value;
        }
    }
}