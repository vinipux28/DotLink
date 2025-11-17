using DotLink.Application.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace DotLink.Application.Features.Users.UnfollowUser
{
    public class UnfollowUserCommandHandler : IRequestHandler<UnfollowUserCommand, Unit>
    {
        private readonly IUserRepository _userRepository;

        public UnfollowUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(UnfollowUserCommand request, CancellationToken cancellationToken)
        {
            await _userRepository.UnfollowAsync(request.FollowerId, request.FolloweeId);
            return Unit.Value;
        }
    }
}
