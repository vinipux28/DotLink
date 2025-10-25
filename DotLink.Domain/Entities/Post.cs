using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotLink.Domain.Entities
{
    public class Post
    {
        public Guid Id { get; init; }
        public string Title { get; private set; } = String.Empty;
        public string Content { get; private set; } = String.Empty;
        public Guid AuthorId { get; private set; }
        public User? Author { get; private set; } = null!;
        public ICollection<Comment> Comments { get; private set; } = new List<Comment>();
        public ICollection<PostVote> PostVotes { get; private set; } = new List<PostVote>();

        private Post() { }

        public Post(Guid id, Guid authorId, string title, string content)
        {
            Id = id;
            Title = title;
            Content = content;
            AuthorId = authorId;
        }
        public void UpdateTitle(string newTitle)
        {
            if (string.IsNullOrWhiteSpace(newTitle))
            {
                throw new ArgumentException("Title cannot be empty.", nameof(newTitle));
            }
            Title = newTitle;
            UpdatedAt = DateTime.UtcNow;
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
