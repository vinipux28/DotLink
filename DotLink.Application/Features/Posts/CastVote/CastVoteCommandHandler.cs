using DotLink.Application.Repositories;
using DotLink.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DotLink.Application.Features.Posts.CastVote
{
    public class CastVoteCommandHandler : IRequestHandler<CastVoteCommand, Unit>
    {
        private readonly IPostVoteRepository _voteRepository;
        private readonly IPostRepository _postRepository;
        private readonly ILogger<CastVoteCommandHandler> _logger;

        public CastVoteCommandHandler(IPostVoteRepository voteRepository, IPostRepository postRepository, ILogger<CastVoteCommandHandler> logger)
        {
            _voteRepository = voteRepository;
            _postRepository = postRepository;
            _logger = logger;
        }

        public async Task<Unit> Handle(CastVoteCommand request, CancellationToken cancellationToken)
        {
            var postExists = await _postRepository.GetByIdAsync(request.PostId);
            if (postExists == null)
            {
                _logger.LogWarning("Vote attempted for non-existing post {PostId} by user {UserId}", request.PostId, request.UserId);
                throw new DotLink.Application.Exceptions.DotLinkNotFoundException("Post", request.PostId);
            }

            if (request.IsUpvote is null)
            {
                await _voteRepository.RemoveVoteAsync(
                    new PostVote(request.PostId, request.UserId, true)
                );

                _logger.LogInformation("Vote removed for post {PostId} by user {UserId}", request.PostId, request.UserId);
                return Unit.Value;
            }

            var vote = new PostVote(
                request.PostId,
                request.UserId,
                (bool)request.IsUpvote
            );

            await _voteRepository.CastVoteAsync(vote);

            _logger.LogInformation("User {UserId} cast {VoteType} on post {PostId}", request.UserId, (request.IsUpvote == true ? "upvote" : "downvote"), request.PostId);

            return Unit.Value;
        }
    }
}