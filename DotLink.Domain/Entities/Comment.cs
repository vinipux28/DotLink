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

        public Guid? ParentCommentId { get; private set; }
        public Comment? ParentComment { get; private set; }
        public ICollection<Comment> Replies { get; private set; } = new List<Comment>();

        private Comment() { }
        public Comment(Guid id, string content, Guid authorId, Guid postId, Guid? parentCommentId = null)
        {
            Id = id;
            Content = content;
            AuthorId = authorId;
            PostId = postId;
            ParentCommentId = parentCommentId;
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
