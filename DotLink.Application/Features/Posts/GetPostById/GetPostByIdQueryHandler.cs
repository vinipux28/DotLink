using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using DotLink.Application.DTOs;
using DotLink.Application.Repositories;
using DotLink.Domain.Entities;

namespace DotLink.Application.Features.Posts.GetPostById
{
    public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, PostDTO>
    {
        private readonly IPostRepository _postRepository;
        public GetPostByIdQueryHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        /*private static List<CommentDTO> BuildCommentTree(List<Comment> allComments)
        {
            var dictionary = allComments.ToDictionary(
                c => c.Id,
                c => new CommentDTO(c)
            );

            var rootComments = new List<CommentDTO>();

            foreach (var commentDto in dictionary.Values)
            {
                if (commentDto.ParentCommentId.HasValue)
                {
                    if (dictionary.TryGetValue(commentDto.ParentCommentId.Value, out var parentDto))
                    {
                        parentDto.Replies.Add(commentDto);
                    }
                }
                else
                {
                    rootComments.Add(commentDto);
                }
            }

            return rootComments;
        }*/

        public async Task<PostDTO> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            var post = await _postRepository.GetByIdAsync(request.PostId, request.IncludeComments);
            if (post == null)
            {
                throw new Exception($"Post with ID {request.PostId} not found.");
            }

            var postDTO = new PostDTO(post);

            /*if (request.IncludeComments && post.Comments != null && post.Comments.Any())
            {
                var allCommentsList = post.Comments.ToList();

                postDTO.Comments = BuildCommentTree(allCommentsList);
            }*/

            return postDTO;
        }
    }
}
