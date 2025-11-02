using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotLink.Application.Features.Comments.UpdateComment
{
    public class UpdateCommentCommand
    {
        public Guid CommentId { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
