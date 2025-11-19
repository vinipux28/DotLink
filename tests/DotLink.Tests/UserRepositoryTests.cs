using System;
using System.Threading.Tasks;
using DotLink.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;
using DotLink.Infrastructure.Repositories;
using DotLink.Domain.Entities;

namespace DotLink.Tests
{
    public class UserRepositoryTests
    {
        private DotLinkDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<DotLinkDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new DotLinkDbContext(options);
        }

        [Fact]
        public async Task Follow_Unfollow_IsFollowing_Works()
        {
            using var context = CreateInMemoryContext();
            var repo = new UserRepository(context);

            var user1 = new User(Guid.NewGuid(), "user1", "u1@example.com", "h1");
            var user2 = new User(Guid.NewGuid(), "user2", "u2@example.com", "h2");

            await repo.AddAsync(user1);
            await repo.AddAsync(user2);

            await repo.FollowAsync(user1.Id, user2.Id);

            var isFollowing = await repo.IsFollowingAsync(user1.Id, user2.Id);
            Assert.True(isFollowing);

            var followers = await repo.GetFollowersAsync(user2.Id);
            Assert.Contains(followers, f => f.FollowerId == user1.Id && f.FolloweeId == user2.Id);

            await repo.UnfollowAsync(user1.Id, user2.Id);

            isFollowing = await repo.IsFollowingAsync(user1.Id, user2.Id);
            Assert.False(isFollowing);
        }
    }
}
