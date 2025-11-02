using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLink.Domain.Entities;
using DotLink.Application.Repositories;
using DotLink.Infrastructure.Data;

namespace DotLink.Infrastructure.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DotLinkDbContext _context;
        public CommentRepository(DotLinkDbContext context)
        {
            _context = context;
        }

        public async Task<Comment?> GetByIdAsync(Guid id)
        {
            return await _context.Comments.FindAsync(id);
        }

        public Task AddAsync(Comment comment)
        {
            _context.Comments.Add(comment);
            return _context.SaveChangesAsync();
        }

        public Task UpdateAsync(Comment comment)
        {
            _context.Comments.Update(comment);
            return _context.SaveChangesAsync();
        }

        public Task DeleteAsync(Comment comment)
        {
            _context.Comments.Remove(comment);
            return _context.SaveChangesAsync();
        }
    }
}
