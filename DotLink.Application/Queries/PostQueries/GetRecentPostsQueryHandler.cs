using DotLink.Application.Repositories;
using DotLink.Application.DTOs;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DotLink.Application.Queries.PostQueries
{
    public class GetRecentPostsQueryHandler : IRequestHandler<GetRecentPostsQuery, IEnumerable<PostDTO>>
    {
        private readonly IPostRepository _postRepository;

        public GetRecentPostsQueryHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<IEnumerable<PostDTO>> Handle(GetRecentPostsQuery request, CancellationToken cancellationToken)
        {
            var posts = await _postRepository.GetRecentPostsAsync(request.Skip, request.Take);

            return posts.Select(p => new PostDTO
            {
                Id = p.Id,
                Title = p.Title,
                Content = p.Content,
                CreatedAt = p.CreatedAt,
                AuthorUsername = p.Author?.Username ?? "Unknown" // TO DO: Make this property of type UserDto
            }).ToList();
        }
    }
}