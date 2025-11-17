using DotLink.Application.Repositories;
using DotLink.Domain.Entities;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotLink.Application.Features.Search
{
    public class CompositeSearchQueryHandler : IRequestHandler<CompositeSearchQuery, List<SearchResultItem>>
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public CompositeSearchQueryHandler(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task<List<SearchResultItem>> Handle(CompositeSearchQuery request, CancellationToken cancellationToken)
        {
            var normalizedSearchTerm = request.SearchTerm.ToLower();

            var users = SearchUsersInNewScope(normalizedSearchTerm);
            var posts = SearchPostsInNewScope(normalizedSearchTerm);
            await Task.WhenAll(
                users,
                posts
            );

            var userResults = (await users).Select(u => MapUserToSearchResultItem(u, normalizedSearchTerm, request.CurrentUserId));
            var postResults = (await posts).Select(p => MapPostToSearchResultItem(p, normalizedSearchTerm, request.CurrentUserId));

            var combinedResults = userResults
                                        .Concat(postResults)
                                        .OrderByDescending(r => r.RelevanceScore)
                                        .Take(50)
                                        .ToList();

            return combinedResults;

        }

        private async Task<List<User>> SearchUsersInNewScope(string searchTerm)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                return await repo.SearchUsersAsync(searchTerm);
            }
        }

        private async Task<List<Post>> SearchPostsInNewScope(string searchTerm)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IPostRepository>();
                return await repo.SearchPostsAsync(searchTerm);
            }
        }


        private SearchResultItem MapUserToSearchResultItem(User user, string normalizedSearchTerm, Guid? currentUserId)
        {
            var relevanceScore = 0.5;
            if (user.Username.ToLower() == normalizedSearchTerm) relevanceScore += 0.3;
            if ((user.FirstName + " " + user.LastName).ToLower().Contains(normalizedSearchTerm)) relevanceScore += 0.2;
            if (user.Bio != null && user.Bio.ToLower().Contains(normalizedSearchTerm)) relevanceScore += 0.2;
            relevanceScore += 0.1 * Math.Sqrt(user.Posts.Count);

            // boost if current user follows this user
            if (currentUserId.HasValue)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var repo = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                    var isFollowing = repo.IsFollowingAsync(currentUserId.Value, user.Id).GetAwaiter().GetResult();
                    if (isFollowing) relevanceScore += 0.25; // boost weight for followed users
                }
            }

            return new SearchResultItem(
                user.Id,
                EntityType.User,
                user.Username,
                (user.Bio is not null && user.Bio.Length > 100) ? user.Bio.Substring(0, 100) + "..." : user.Bio,
                relevanceScore,
                user.ProfilePictureKey
            );
        }

        private SearchResultItem MapPostToSearchResultItem(Post post, string normalizedSearchTerm, Guid? currentUserId)
        {
            var relevanceScore = 0.5;
            if (post.Title.ToLower() == normalizedSearchTerm) relevanceScore += 0.2;
            if (post.Content.ToLower().Contains(normalizedSearchTerm)) relevanceScore += 0.1;
            relevanceScore += 0.1 * Math.Sqrt(post.PostVotes.Count);

            // boost if current user follows the post's author
            if (currentUserId.HasValue)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var repo = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                    var isFollowingAuthor = repo.IsFollowingAsync(currentUserId.Value, post.AuthorId).GetAwaiter().GetResult();
                    if (isFollowingAuthor) relevanceScore += 0.2; // smaller boost for posts by followed authors
                }
            }

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
