using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotLink.Domain.Entities
{
    public class PostVote
    {
        public Guid PostId { get; private set; }
        public Post? Post { get; private set; } = null!;
        public Guid UserId { get; private set; }
        public User? User { get; private set; } = null!;
        public bool IsUpvote { get; private set; }
        private PostVote() { }
        public PostVote(Guid postId, Guid userId, bool isUpvote)
        {
            PostId = postId;
            UserId = userId;
            IsUpvote = isUpvote;
        }
        public void ChangeVote(bool isUpvote)
        {
            IsUpvote = isUpvote;
        }
    }
}
