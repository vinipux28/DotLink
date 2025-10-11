using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DotLink.Domain.Entities
{
     public class User
    {
        public int Id { get; init; }
        public string Username { get; private set; } = String.Empty;
        public string Email { get; private set; } = String.Empty;
        public string PasswordHash { get; private set; } = String.Empty;

        public ICollection<Post> Posts { get; private set; } = new List<Post>();
        public ICollection<Comment> Comments { get; private set; } = new List<Comment>();
        // Если вы реализуете лайки/голоса
        public ICollection<PostVote> Votes { get; private set; } = new List<PostVote>();

        private User() { }
        public User(string username, string email, string passwordHash)
        {
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateUsername(string newUsername)
        {
            if (string.IsNullOrWhiteSpace(newUsername))
            {
                throw new ArgumentException("Username cannot be empty.", nameof(newUsername));
            }
            Username = newUsername;
            UpdatedAt = DateTime.UtcNow;
        }


        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; private set; }
    }
}
