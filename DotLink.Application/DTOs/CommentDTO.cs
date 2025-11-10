using System;
using DotLink.Domain.Entities;

namespace DotLink.Application.DTOs
{
    public class CommentDTO
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserDTOPartial Author { get; set; }

        public Guid? ParentCommentId { get; set; }

        public CommentDTO(Comment comment)
        {
            Id = comment.Id;
            Content = comment.Content;
            CreatedAt = comment.CreatedAt;
            Author = new UserDTOPartial(comment.Author);
            ParentCommentId = comment.ParentCommentId;
        }
    }
}