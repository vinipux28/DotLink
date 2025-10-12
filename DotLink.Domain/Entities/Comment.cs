using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotLink.Domain.Entities
{
    public class Comment
    {
        public Guid Id { get; init; }
        public string Content { get; private set; } = String.Empty;
        public Guid AuthorId { get; private set; }
        public User? Author { get; private set; } = null!;
        public Guid PostId { get; private set; }
        public Post? Post { get; private set; } = null!;
        private Comment() { }
        public Comment(string content, Guid authorId, Guid postId)
        {
            Content = content;
            AuthorId = authorId;
            PostId = postId;
        }
        public void UpdateContent(string newContent)
        {
            if (string.IsNullOrWhiteSpace(newContent))
            {
                throw new ArgumentException("Content cannot be empty.", nameof(newContent));
            }
            Content = newContent;
            UpdatedAt = DateTime.UtcNow;
        }
        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; private set; }
    }
}
