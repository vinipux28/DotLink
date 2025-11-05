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
        Task<(List<Comment> Comments, int TotalCount)> GetPaginatedByPostIdAsync(
            Guid postId,
            int pageNumber,
            int pageSize
        );
        Task<(List<Comment> Comments, int TotalCount)> GetPaginatedRepliesAsync(
            Guid parentCommentId,
            int pageNumber,
            int pageSize
        );
        public Task AddAsync(Comment comment);
        public Task UpdateAsync(Comment comment);
        public Task DeleteAsync(Comment comment);
    }
}
