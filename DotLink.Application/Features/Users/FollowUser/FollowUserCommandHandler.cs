using DotLink.Application.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DotLink.Application.Features.Users.FollowUser
{
    public class FollowUserCommandHandler : IRequestHandler<FollowUserCommand, Unit>
    {
        private readonly IUserRepository _userRepository;

        public FollowUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(FollowUserCommand request, CancellationToken cancellationToken)
        {
            await _userRepository.FollowAsync(request.FollowerId, request.FolloweeId);
            return Unit.Value;
        }
    }
}
