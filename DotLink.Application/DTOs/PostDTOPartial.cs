using System;

namespace DotLink.Application.DTOs
{
    public class PostDTOPartial
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public UserDTOPartial Author { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}