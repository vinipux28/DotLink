using DotLink.Application.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DotLink.Application.Features.Users.GetFollowings
{
    public class GetFollowingsQueryHandler : IRequestHandler<GetFollowingsQuery, List<Guid>>
    {
        private readonly IUserRepository _userRepository;

        public GetFollowingsQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<Guid>> Handle(GetFollowingsQuery request, CancellationToken cancellationToken)
        {
            var followings = await _userRepository.GetFollowingsAsync(request.UserId);
            return followings.Select(f => f.FolloweeId).ToList();
        }
    }
}
