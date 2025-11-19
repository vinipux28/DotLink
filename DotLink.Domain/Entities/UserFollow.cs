using System;

namespace DotLink.Domain.Entities
{
    public class UserFollow
    {
        public Guid FollowerId { get; private set; }
        public User Follower { get; private set; } = null!;

        public Guid FolloweeId { get; private set; }
        public User Followee { get; private set; } = null!;

        private UserFollow() { }

        public UserFollow(Guid followerId, Guid followeeId)
        {
            if (followerId == Guid.Empty) throw new ArgumentException("followerId");
            if (followeeId == Guid.Empty) throw new ArgumentException("followeeId");
            FollowerId = followerId;
            FolloweeId = followeeId;
        }
    }
}
