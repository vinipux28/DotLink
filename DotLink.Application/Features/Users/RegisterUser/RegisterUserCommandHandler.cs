using DotLink.Application.Repositories;
using DotLink.Domain.Entities;
using MediatR;
using DotLink.Application.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DotLink.Application.Features.Users.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<RegisterUserCommandHandler> _logger;

        public RegisterUserCommandHandler(IUserRepository userRepository, ILogger<RegisterUserCommandHandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                _logger.LogWarning("Registration attempt with already used email {Email}", request.Email);
                throw new DotLinkConflictException("User", "email", request.Email);
            }

            existingUser = await _userRepository.GetByUsernameAsync(request.Username);
            if (existingUser != null) {
                _logger.LogWarning("Registration attempt with already used username {Username}", request.Username);
                throw new DotLinkConflictException("User", "username", request.Username);
            }

            var passwordHash = User.HashPassword(request.Password);

            var newUser = new User(
                Guid.NewGuid(),
                request.Username,
                request.Email,
                passwordHash
            );

            await _userRepository.AddAsync(newUser);

            _logger.LogInformation("User {UserId} registered with username {Username}", newUser.Id, newUser.Username);

            return newUser.Id;
        }

    }
}