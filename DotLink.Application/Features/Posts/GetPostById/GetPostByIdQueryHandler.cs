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
using Microsoft.Extensions.Logging;
using DotLink.Application.Services;

namespace DotLink.Application.Features.Posts.GetPostById
{
    public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, PostDTO>
    {
        private readonly IPostRepository _postRepository;
        private readonly ILogger<GetPostByIdQueryHandler> _logger;
        private readonly IDTOMapperService _mapperService;
        public GetPostByIdQueryHandler(IPostRepository postRepository, ILogger<GetPostByIdQueryHandler> logger, IDTOMapperService mapperService)
        {
            _postRepository = postRepository;
            _logger = logger;
            _mapperService = mapperService;
        }


        public async Task<PostDTO> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            var post = await _postRepository.GetByIdAsync(request.PostId);
            if (post == null)
            {
                _logger.LogWarning("Post {PostId} not found", request.PostId);
                throw new DotLinkNotFoundException("Post", request.PostId);
            }

            var postDTO = _mapperService.MapToPostDTO(post);

            _logger.LogInformation("Post {PostId} retrieved", request.PostId);

            return postDTO;
        }
    }
}
