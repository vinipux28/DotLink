using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotLink.Domain.Entities;

namespace DotLink.Application.Repositories
{
    public interface ICommentRepository
    {
        public Task AddAsync(Comment comment);
    }
}
