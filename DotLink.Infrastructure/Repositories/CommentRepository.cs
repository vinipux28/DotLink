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

        public Task AddAsync(Comment comment)
        {
            _context.Comments.Add(comment);
            return _context.SaveChangesAsync();
        }
    }
}
