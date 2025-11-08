using DotLink.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotLink.Application.DTOs
{
    public class UserDTOPartial
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string? ProfilePictureKey { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }


        public UserDTOPartial(User? user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null");
            }
            Id = user.Id;
            Username = user.Username;
            ProfilePictureKey = user.ProfilePictureKey;

            CreatedAt = user.CreatedAt;
            UpdatedAt = user.UpdatedAt;
        }
    }
}
