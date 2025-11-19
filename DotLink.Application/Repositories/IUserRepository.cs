using DotLink.Domain.Entities;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DotLink.Application.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid userId);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUsernameAsync(string username);
        Task<List<User>> SearchUsersAsync(string searchTerm);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task<bool> IsUsernameUniqueAsync(string username);

        // Follow/Unfollow operations
        Task FollowAsync(Guid followerId, Guid followeeId);
        Task UnfollowAsync(Guid followerId, Guid followeeId);
        Task<bool> IsFollowingAsync(Guid followerId, Guid followeeId);

        // Retrieve follower relations
        Task<List<UserFollow>> GetFollowersAsync(Guid userId);
        Task<List<UserFollow>> GetFollowingsAsync(Guid userId);
    }
}