using DotLink.Application.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotLink.Application.Features.Search
{
    public class CompositeSearchQueryHandler : IRequestHandler<CompositeSearchQuery, List<SearchResultItem>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;
        public CompositeSearchQueryHandler(IUserRepository userRepository, IPostRepository postRepository)
        {
            _userRepository = userRepository;
            _postRepository = postRepository;
        }

        public async Task<List<SearchResultItem>> Handle(CompositeSearchQuery request, CancellationToken cancellationToken)
        {
            var users = _userRepository.SearchUsersAsync(request.SearchTerm);
            var posts = _postRepository.SearchPostsAsync(request.SearchTerm);
            await Task.WhenAll(
                users,
                posts
            );

            var combinedResults = users.Result
                                        .Select(u => new SearchResultItem(u))
                                        .Concat(
                                            posts.Result.Select(p => new SearchResultItem(p))
                                        )
                                        .OrderByDescending(r => r.RelevanceScore)
                                        .Take(50)
                                        .ToList();

            return combinedResults;

        }
    }
}
