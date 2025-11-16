using DotLink.Application.Repositories;
using DotLink.Application.DTOs;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DotLink.Application.Features.Posts.GetRecentPosts
{
    public class GetRecentPostsQueryHandler : IRequestHandler<GetRecentPostsQuery, IEnumerable<PostDTOPartial>>
    {
        private readonly IPostRepository _postRepository;
        private readonly ILogger<GetRecentPostsQueryHandler> _logger;

        public GetRecentPostsQueryHandler(IPostRepository postRepository, ILogger<GetRecentPostsQueryHandler> logger)
        {
            _postRepository = postRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<PostDTOPartial>> Handle(GetRecentPostsQuery request, CancellationToken cancellationToken)
        {
            var posts = await _postRepository.GetRecentPostsAsync(request.Skip, request.Take);

            var result = posts.Select(p => new PostDTOPartial
            {
                Id = p.Id,
                Title = p.Title,
                Content = p.Content,
                CreatedAt = p.CreatedAt,
                Author = new UserDTOPartial(p.Author)
            }).ToList();

            _logger.LogInformation("Retrieved {Count} recent posts (skip={Skip}, take={Take})", result.Count, request.Skip, request.Take);

            return result;
        }
    }
}