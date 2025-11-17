using DotLink.Application.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DotLink.Application.Features.Users.GetFollowers
{
    public class GetFollowersQueryHandler : IRequestHandler<GetFollowersQuery, List<Guid>>
    {
        private readonly IUserRepository _userRepository;

        public GetFollowersQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<Guid>> Handle(GetFollowersQuery request, CancellationToken cancellationToken)
        {
            var followers = await _userRepository.GetFollowersAsync(request.UserId);
            return followers.Select(f => f.FollowerId).ToList();
        }
    }
}
