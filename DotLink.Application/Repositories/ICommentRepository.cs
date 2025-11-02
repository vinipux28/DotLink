using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLink.Domain.Entities;

namespace DotLink.Application.Repositories
{
    public interface ICommentRepository
    {
        public Task<Comment?> GetByIdAsync(Guid id);
        public Task AddAsync(Comment comment);
        public Task UpdateAsync(Comment comment);
        public Task DeleteAsync(Comment comment);
    }
}
