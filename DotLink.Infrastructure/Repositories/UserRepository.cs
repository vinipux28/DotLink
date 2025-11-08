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
    }
}
