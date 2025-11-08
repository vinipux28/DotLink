using DotLink.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotLink.Application.DTOs
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string? Bio { get; set; }
        public string? ProfilePictureKey { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }


        public UserDTO(User? user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null");
            }
            Id = user.Id;
            Username = user.Username;
            Email = user.Email;
            Bio = user.Bio;
            ProfilePictureKey = user.ProfilePictureKey;

            CreatedAt = user.CreatedAt;
            UpdatedAt = user.UpdatedAt;
        }
    }
}
