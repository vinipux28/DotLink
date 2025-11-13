using DotLink.Application.Repositories;
using DotLink.Domain.Entities;
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
            var normalizedSearchTerm = request.SearchTerm.ToLower();

            var users = _userRepository.SearchUsersAsync(normalizedSearchTerm);
            var posts = _postRepository.SearchPostsAsync(normalizedSearchTerm);
            await Task.WhenAll(
                users,
                posts
            );

            var combinedResults = (await users)
                                        .Select(u => MapUserToSearchResultItem(u, normalizedSearchTerm))
                                        .Concat(
                                            (await posts).Select(p => MapPostToSearchResultItem(p, normalizedSearchTerm))
                                        )
                                        .OrderByDescending(r => r.RelevanceScore)
                                        .Take(50)
                                        .ToList();

            return combinedResults;

        }


        private SearchResultItem MapUserToSearchResultItem(User user, string normalizedSearchTerm)
        {
            var relevanceScore = 0.5;
            if (user.Username.ToLower() == normalizedSearchTerm) relevanceScore += 0.3;
            if ((user.FirstName + " " + user.LastName).ToLower().Contains(normalizedSearchTerm)) relevanceScore += 0.2;
            if (user.Bio != null && user.Bio.ToLower().Contains(normalizedSearchTerm)) relevanceScore += 0.2;
            relevanceScore += 0.1 * Math.Sqrt(user.Posts.Count);

            return new SearchResultItem(
                user.Id,
                EntityType.User,
                user.Username,
                (user.Bio is not null && user.Bio.Length > 100) ? user.Bio.Substring(0, 100) + "..." : user.Bio,
                relevanceScore,
                user.ProfilePictureKey
            );
        }

        private SearchResultItem MapPostToSearchResultItem(Post post, string normalizedSearchTerm)
        {
            var relevanceScore = 0.5;
            if (post.Title.ToLower() == normalizedSearchTerm) relevanceScore += 0.2;
            if (post.Content.ToLower().Contains(normalizedSearchTerm)) relevanceScore += 0.1;
            relevanceScore += 0.1 * Math.Sqrt(post.PostVotes.Count);

            return new SearchResultItem(
                post.Id,
                EntityType.Post,
                post.Title,
                post.Content.Length > 100 ? post.Content.Substring(0, 100) + "..." : post.Content,
                relevanceScore,
                null
            );  
        }
    }
}
