using DotLink.Application.Repositories;
using DotLink.Application.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DotLink.Application.Commands.UserCommands
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public LoginUserCommandHandler(IUserRepository userRepository, IJwtService jwtService )
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Incorrect login or password");
            }

            bool isPasswordValid = user.VerifyPassword(request.Password);

            if (!isPasswordValid)
            {
                throw new UnauthorizedAccessException("Incorrect login or password");
            }

            string token = _jwtService.GenerateToken(user);

            return token;
        }
    }
}