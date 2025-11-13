using DotLink.Domain.Entities;
using DotLink.Infrastructure.Data;
using DotLink.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DotLink.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly DotLinkDbContext _context;


        public PostRepository(DotLinkDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
        }

        public async Task<Post?> GetByIdAsync(Guid postId)
        {
            return await _context.Posts
                    .Include(p => p.Author)
                    .FirstOrDefaultAsync(p => p.Id == postId);
        }

        public async Task<List<Post>> SearchPostsAsync(string normizedSearchTerm)
        {
            return await _context.Posts
                .AsNoTracking()
                .Include(p => p.Author)
                .Include(p => p.PostVotes)
                .Where(p => p.Title.ToLower().Contains(normizedSearchTerm) || p.Content.ToLower().Contains(normizedSearchTerm))
                .ToListAsync();
        }

        public Task UpdateAsync(Post post)
        {
            _context.Posts.Update(post);
            return _context.SaveChangesAsync();
        }

        public Task DeleteAsync(Post post)
        {
            _context.Posts.Remove(post);
            return _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Post>> GetRecentPostsAsync(int skip, int take)
        {
            return await _context.Posts
                                 .Include(p => p.Author)
                                 .OrderByDescending(p => p.CreatedAt)
                                 .Skip(skip)    
                                 .Take(take)
                                 .AsNoTracking() // read-only optimization
                                 .ToListAsync();                
        }


    }
}
