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

        public Task<Post?> GetByIdAsync(Guid postId)
        {
            return _context.Posts
                .Include(p => p.Author)
                .FirstOrDefaultAsync(p => p.Id == postId);
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

        public Task<Post?> GetPostWithDetailsAsync(Guid postId)
        {
            return _context.Posts.Include(p => p.Author)
                                 .Include(p => p.Comments)
                                 .Include(p => p.PostVotes)
                                 .FirstOrDefaultAsync(p => p.Id == postId);
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
