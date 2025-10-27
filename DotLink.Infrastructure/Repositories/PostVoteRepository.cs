using DotLink.Application.Repositories;
using DotLink.Domain.Entities;
using DotLink.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DotLink.Infrastructure.Repositories
{
    public class PostVoteRepository : IPostVoteRepository
    {
        private readonly DotLinkDbContext _context;

        public PostVoteRepository(DotLinkDbContext context)
        {
            _context = context;
        }

        public Task<PostVote?> GetVoteAsync(Guid userId, Guid postId)
        {
            return _context.PostVotes
                .FirstOrDefaultAsync(v => v.UserId == userId && v.PostId == postId);
        }

        public async Task CastVoteAsync(PostVote vote)
        {
            var existingVote = await GetVoteAsync(vote.UserId, vote.PostId);

            if (existingVote == null)
            {
                await _context.PostVotes.AddAsync(vote);
            }
            else
            {
                existingVote.SetIsUpvote(vote.IsUpvote);
                _context.PostVotes.Update(existingVote);
            }

            await _context.SaveChangesAsync();
        }

        public Task RemoveVoteAsync(PostVote vote)
        {
            _context.PostVotes.Remove(vote);
            return _context.SaveChangesAsync();
        }
    }
}