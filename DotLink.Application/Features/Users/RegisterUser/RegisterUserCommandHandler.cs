using DotLink.Application.Repositories;
using DotLink.Domain.Entities;
using MediatR;
using DotLink.Application.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DotLink.Application.Features.Users.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Guid>
    {
        private readonly IUserRepository _userRepository;

        public RegisterUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByEmailAsync(request.Email);

            if (existingUser != null)
            {
                throw new DotLinkConflictException("User", "email", request.Email);
            }

            var passwordHash = User.HashPassword(request.Password);

            var newUser = new User(
                Guid.NewGuid(),
                request.Username,
                request.Email,
                passwordHash
            );

            await _userRepository.AddAsync(newUser);

            return newUser.Id;
        }

    }
}