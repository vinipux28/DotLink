using DotLink.Domain.Entities;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace DotLink.Application.Repositories
{
    public interface IPostRepository
    {
        Task<Post?> GetByIdAsync(Guid postId);

        Task AddAsync(Post post);

        Task UpdateAsync(Post post);

        Task DeleteAsync(Post post);

        Task<Post?> GetPostWithDetailsAsync(Guid postId);

        Task<IEnumerable<Post>> GetRecentPostsAsync(int skip, int take);
    }
}