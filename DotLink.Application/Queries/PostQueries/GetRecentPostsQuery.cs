using MediatR;
using DotLink.Application.DTOs;
using System.Collections.Generic;

namespace DotLink.Application.Queries.PostQueries
{
    public class GetRecentPostsQuery : IRequest<IEnumerable<PostDTO>>
    {
        private const int MaxPageSize = 50;

        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public int PageNumber { get; set; } = 1;

        public int Skip => (PageNumber - 1) * PageSize;

        public int Take => PageSize;
    }
}