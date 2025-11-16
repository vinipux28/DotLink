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

        public CommentDTO()
        {}
    }
}