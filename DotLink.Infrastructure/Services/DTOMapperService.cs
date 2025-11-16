using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DotLink.Application.DTOs;
using DotLink.Domain.Entities;
using DotLink.Application.Services;

namespace DotLink.Infrastructure.Services
{
    public class DTOMapperService : IDTOMapperService
    {
        public DTOMapperService() { }

        public static UserDTO MapToUserDTO(User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Bio = user.Bio,
                ProfilePictureKey = user.ProfilePictureKey,
                CreatedAt = user.CreatedAt
            };
        }

        public static UserDTOPartial MapToUserDTOPartial(User user)
        {
            return new UserDTOPartial
            {
                Id = user.Id,
                Username = user.Username,
                ProfilePictureKey = user.ProfilePictureKey
            };
        }

        public static PostDTO MapToPostDTO(Post post)
        {
            return new PostDTO
            {
                Id = post.Id,
                Content = post.Content,
                Author = MapToUserDTOPartial(post.Author),
                CreatedAt = post.CreatedAt,
                UpdatedAt = post.UpdatedAt ?? post.CreatedAt,
            };
        }

        public static PostDTOPartial MapToPostDTOPartial(Post post)
        {
            return new PostDTOPartial
            {
                Id = post.Id,
                Content = post.Content,
                CreatedAt = post.CreatedAt,
            };
        }

        public static CommentDTO MapToCommentDTO(Comment comment)
        {
            return new CommentDTO
            {
                Id = comment.Id,
                Content = comment.Content,
                Author = MapToUserDTOPartial(comment.Author),
                CreatedAt = comment.CreatedAt
            };
        }
    }
}
