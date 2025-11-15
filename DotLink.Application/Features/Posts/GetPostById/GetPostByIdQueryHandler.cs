using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using DotLink.Application.DTOs;
using DotLink.Application.Repositories;
using DotLink.Domain.Entities;
using DotLink.Application.Exceptions;

namespace DotLink.Application.Features.Posts.GetPostById
{
    public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, PostDTO>
    {
        private readonly IPostRepository _postRepository;
        public GetPostByIdQueryHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }


        public async Task<PostDTO> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            var post = await _postRepository.GetByIdAsync(request.PostId);
            if (post == null)
            {
                throw new DotLinkNotFoundException("Post", request.PostId);
            }

            var postDTO = new PostDTO(post);

            return postDTO;
        }
    }
}
