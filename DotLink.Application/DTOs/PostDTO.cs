using System;

namespace DotLink.Application.DTOs
{
    public class PostDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string AuthorUsername { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}