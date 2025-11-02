using System;
using DotLink.Domain.Entities;

namespace DotLink.Application.DTOs
{
    public class CommentDTO
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserDTO Author { get; set; }

        public Guid? ParentCommentId { get; set; }
        public List<CommentDTO> Replies { get; set; } = new List<CommentDTO>();

        public CommentDTO(Comment comment)
        {
            Id = comment.Id;
            Content = comment.Content;
            CreatedAt = comment.CreatedAt;
            Author = new UserDTO(comment.Author);
        }
    }
}