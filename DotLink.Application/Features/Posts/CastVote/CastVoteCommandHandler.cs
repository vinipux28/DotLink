using DotLink.Application.Repositories;
using DotLink.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DotLink.Application.Features.Posts.CastVote
{
    public class CastVoteCommandHandler : IRequestHandler<CastVoteCommand, Unit>
    {
        private readonly IPostVoteRepository _voteRepository;
        private readonly IPostRepository _postRepository;

        public CastVoteCommandHandler(IPostVoteRepository voteRepository, IPostRepository postRepository)
        {
            _voteRepository = voteRepository;
            _postRepository = postRepository;
        }

        public async Task<Unit> Handle(CastVoteCommand request, CancellationToken cancellationToken)
        {
            var postExists = await _postRepository.GetByIdAsync(request.PostId);
            if (postExists == null)
            {
                throw new Exception($"Post with ID {request.PostId} not found.");
            }

            if (request.IsUpvote is null)
            {
                await _voteRepository.RemoveVoteAsync(
                    new PostVote(request.PostId, request.UserId, true)
                );
                return Unit.Value;
            }

            var vote = new PostVote(
                request.PostId,
                request.UserId,
                (bool)request.IsUpvote
            );

            await _voteRepository.CastVoteAsync(vote);

            return Unit.Value;
        }
    }
}