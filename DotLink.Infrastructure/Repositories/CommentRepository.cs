using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLink.Domain.Entities;
using DotLink.Application.Repositories;
using DotLink.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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

        public async Task<(List<Comment> Comments, int TotalCount)> GetPaginatedByPostIdAsync(
            Guid postId,
            int pageNumber,
            int pageSize)
        {
            var baseQuery = _context.Comments
                .Where(c => c.PostId == postId)
                .Where(c => c.ParentCommentId == null)
                .Include(c => c.Author);

            var totalCount = await baseQuery.CountAsync();

            var comments = await baseQuery
                .OrderByDescending(c => c.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (comments, totalCount);
        }

        public async Task<(List<Comment> Comments, int TotalCount)> GetPaginatedRepliesAsync(
            Guid parentCommentId,
            int pageNumber,
            int pageSize)
        {
            var baseQuery = _context.Comments
                .Where(c => c.ParentCommentId == parentCommentId)
                .Include(c => c.Author);

            var totalCount = await baseQuery.CountAsync();

            var comments = await baseQuery
                .OrderByDescending(c => c.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (comments, totalCount);
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
