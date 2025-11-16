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


        public UserDTOPartial()
        {}
    }
}
