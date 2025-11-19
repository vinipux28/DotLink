using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using BCrypt.Net;

namespace DotLink.Domain.Entities
{
     public class User
    {
        public Guid Id { get; init; }
        public string FirstName { get; private set; } = String.Empty;
        public string LastName { get; private set; } = String.Empty;
        public string Username { get; private set; } = String.Empty;
        public string Email { get; private set; } = String.Empty;
        public string PasswordHash { get; private set; } = String.Empty;

        public string? ProfilePictureKey { get; private set; }
        public string? Bio { get; private set; }

        public string? PasswordResetToken { get; private set; }
        public DateTime? PasswordResetTokenExpiry { get; private set; }


        public ICollection<Post> Posts { get; private set; } = new List<Post>();
        public ICollection<Comment> Comments { get; private set; } = new List<Comment>();
        public ICollection<PostVote> Votes { get; private set; } = new List<PostVote>();

        // Follow relationships (many-to-many modeled with a join entity UserFollow)
        public ICollection<UserFollow> Followers { get; private set; } = new List<UserFollow>();
        public ICollection<UserFollow> Following { get; private set; } = new List<UserFollow>();

        private User() { }
        public User(Guid id, string username, string email, string passwordHash)
        {
            Id = id;
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateName(string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentException("First name cannot be empty.", nameof(firstName));
            }
            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentException("Last name cannot be empty.", nameof(lastName));
            }
            FirstName = firstName;
            LastName = lastName;
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

        public void UpdateProfilePictureKey(string? key)
        {
            ProfilePictureKey = key;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateBio(string? bio)
        {
            Bio = bio;
            UpdatedAt = DateTime.UtcNow;
        }

        public bool VerifyPassword(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, this.PasswordHash);
        }

        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public void UpdatePassword(string newPassword)
        {
            string newHash = HashPassword(newPassword);

            this.PasswordHash = newHash;
            this.UpdatedAt = DateTime.UtcNow;
        }

        public void SetPasswordResetToken(string token, DateTime expiry)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException("Token cannot be null or empty.", nameof(token));
            }
            this.PasswordResetToken = token;
            this.PasswordResetTokenExpiry = expiry;
            this.UpdatedAt = DateTime.UtcNow;
        }

        public void ClearPasswordResetToken()
        {
            this.PasswordResetToken = null;
            this.PasswordResetTokenExpiry = null;
            this.UpdatedAt = DateTime.UtcNow;
        }

        // Follow management
        public bool IsFollowing(User other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            return Following.Any(f => f.FolloweeId == other.Id);
        }

        public void Follow(User other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            if (other.Id == this.Id) throw new InvalidOperationException("Users cannot follow themselves.");
            if (IsFollowing(other)) return;

            var relation = new UserFollow(this.Id, other.Id);
            this.Following.Add(relation);
            other.Followers.Add(relation);

            UpdatedAt = DateTime.UtcNow;
        }

        public void Unfollow(User other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            var relation = Following.FirstOrDefault(f => f.FolloweeId == other.Id);
            if (relation == null) return;

            Following.Remove(relation);
            other.Followers.Remove(relation);

            UpdatedAt = DateTime.UtcNow;
        }

        public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; private set; }
    }
}
