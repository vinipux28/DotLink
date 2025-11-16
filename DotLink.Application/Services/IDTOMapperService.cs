using DotLink.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLink.Domain.Entities;

namespace DotLink.Application.Services
{
    public interface IDTOMapperService
    {
        abstract static UserDTO MapToUserDTO(User user);

        abstract static UserDTOPartial MapToUserDTOPartial(User user);

        abstract static PostDTO MapToPostDTO(Post post);

        abstract static PostDTOPartial MapToPostDTOPartial(Post post);

        abstract static CommentDTO MapToCommentDTO(Comment comment);

    }
}
