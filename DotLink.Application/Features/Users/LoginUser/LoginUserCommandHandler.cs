using DotLink.Application.Exceptions;
using DotLink.Application.Repositories;
using DotLink.Application.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DotLink.Application.Features.Users.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly ILogger<LoginUserCommandHandler> _logger;

        public LoginUserCommandHandler(IUserRepository userRepository, IJwtService jwtService, ILogger<LoginUserCommandHandler> logger)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _logger = logger;
        }

        public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Term) ?? await _userRepository.GetByUsernameAsync(request.Term);

            if (user == null)
            {
                _logger.LogWarning("Failed login attempt for term {Term}", request.Term);
                throw new DotLinkUnauthorizedAccessException("Incorrect login or password");
            }

            bool isPasswordValid = user.VerifyPassword(request.Password);

            if (!isPasswordValid)
            {
                _logger.LogWarning("Invalid password for user {UserId}", user.Id);
                throw new DotLinkUnauthorizedAccessException("Incorrect login or password");
            }

            string token = _jwtService.GenerateToken(user);

            _logger.LogInformation("User {UserId} logged in", user.Id);

            return token;
        }
    }
}