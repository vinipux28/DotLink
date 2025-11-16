using DotLink.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotLink.Application.DTOs
{
    public class PostDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public UserDTOPartial? Author { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int UpVotes { get; set; }
        public int DownVotes { get; set; }


        public PostDTO()
        {}
    }
}
