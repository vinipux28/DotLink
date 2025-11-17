using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLink.Application.Repositories;
using DotLink.Domain.Entities;
using DotLink.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DotLink.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DotLinkDbContext _context;

        public UserRepository(DotLinkDbContext context)
        {
            _context = context;
        }

        public Task<User?> GetByIdAsync(Guid userId)
        {
            return _context.Users.FindAsync(userId).AsTask();
        }

        public Task<User?> GetByEmailAsync(string email)
        {
            return _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public Task<User?> GetByUsernameAsync(string username)
        {
            return _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<List<User>> SearchUsersAsync(string normalizedSearchTerm)
        {
            return await _context.Users
                .AsNoTracking()
                .Include(u => u.Posts)
                .Where(u => u.Username.ToLower().Contains(normalizedSearchTerm) || (u.FirstName + " " + u.LastName).ToLower().Contains(normalizedSearchTerm))
                .ToListAsync();
        }

        public async Task AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsUsernameUniqueAsync(string username)
        {
            return !await _context.Users
                .AsNoTracking()
                .AnyAsync(u => u.Username == username);
        }

        public async Task FollowAsync(Guid followerId, Guid followeeId)
        {
            if (followerId == followeeId) return;

            var exists = await _context.UserFollows.FindAsync(followerId, followeeId);
            if (exists != null) return;

            var relation = new UserFollow(followerId, followeeId);
            _context.UserFollows.Add(relation);
            await _context.SaveChangesAsync();
        }

        public async Task UnfollowAsync(Guid followerId, Guid followeeId)
        {
            var relation = await _context.UserFollows.FindAsync(followerId, followeeId);
            if (relation == null) return;

            _context.UserFollows.Remove(relation);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsFollowingAsync(Guid followerId, Guid followeeId)
        {
            return await _context.UserFollows.AnyAsync(uf => uf.FollowerId == followerId && uf.FolloweeId == followeeId);
        }
    }
}
