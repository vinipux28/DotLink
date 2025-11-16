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
        abstract public UserDTO MapToUserDTO(User user);

        abstract public UserDTOPartial MapToUserDTOPartial(User user);

        abstract public PostDTO MapToPostDTO(Post post);

        abstract public PostDTOPartial MapToPostDTOPartial(Post post);

        abstract public CommentDTO MapToCommentDTO(Comment comment);

    }
}
