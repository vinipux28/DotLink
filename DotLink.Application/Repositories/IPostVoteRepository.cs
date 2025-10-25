using DotLink.Domain.Entities;
using System.Threading.Tasks;

namespace DotLink.Application.Repositories
{
    public interface IPostVoteRepository
    {
        Task CastVoteAsync(PostVote vote);
        Task<PostVote?> GetVoteAsync(Guid userId, Guid postId);
    }
}