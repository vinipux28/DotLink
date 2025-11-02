using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace DotLink.Application.Features.Comments.DeleteComment
{
    public class DeleteCommentCommand : IRequest<Unit>
    {
        public Guid CommentId { get; set; }
    }
}
