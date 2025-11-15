using DotLink.Application.Exceptions;
using DotLink.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotLink.Application.Features.Comments.UpdateComment
{
    public class UpdateCommentCommandHandler
    {
        private readonly ICommentRepository _commentRepository;
        public UpdateCommentCommandHandler(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await _commentRepository.GetByIdAsync(request.CommentId);
            if (comment is null)
            {
                throw new DotLinkNotFoundException("Comment", request.CommentId);
            }
            comment.UpdateContent(request.Content);
            await _commentRepository.UpdateAsync(comment);
        }

    }
}
