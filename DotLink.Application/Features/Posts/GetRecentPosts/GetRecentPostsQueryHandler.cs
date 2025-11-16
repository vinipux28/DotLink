using DotLink.Application.Repositories;
using DotLink.Application.DTOs;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using DotLink.Application.Services;

namespace DotLink.Application.Features.Posts.GetRecentPosts
{
    public class GetRecentPostsQueryHandler : IRequestHandler<GetRecentPostsQuery, IEnumerable<PostDTOPartial>>
    {
        private readonly IPostRepository _postRepository;
        private readonly ILogger<GetRecentPostsQueryHandler> _logger;
        private readonly IDTOMapperService _mapperService;

        public GetRecentPostsQueryHandler(IPostRepository postRepository, ILogger<GetRecentPostsQueryHandler> logger, IDTOMapperService mapperService)
        {
            _postRepository = postRepository;
            _logger = logger;
            _mapperService = mapperService;
        }

        public async Task<IEnumerable<PostDTOPartial>> Handle(GetRecentPostsQuery request, CancellationToken cancellationToken)
        {
            var posts = await _postRepository.GetRecentPostsAsync(request.Skip, request.Take);

            var result = posts.Select(p => _mapperService.MapToPostDTOPartial(p)).ToList();

            _logger.LogInformation("Retrieved {Count} recent posts (skip={Skip}, take={Take})", result.Count, request.Skip, request.Take);

            return result;
        }
    }
}