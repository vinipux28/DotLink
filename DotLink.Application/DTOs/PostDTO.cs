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
        public UserDTO? Author { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int UpVotes { get; set; }
        public int DownVotes { get; set; }

        public List<CommentDTO> Comments { get; set; } = new List<CommentDTO>();


        public PostDTO(Post post)
        {
            Id = post.Id;
            Title = post.Title;
            Content = post.Content;
            CreatedAt = post.CreatedAt;
            Author = new UserDTO(post.Author);
            UpVotes = post.PostVotes.Count(v => v.IsUpvote);
            DownVotes = post.PostVotes.Count(v => !v.IsUpvote);
            Comments = post.Comments.Select(c => new CommentDTO(c)).ToList();
        }
    }
}
