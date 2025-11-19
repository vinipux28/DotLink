using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotLink.Application.Features.Search
{
    public class CompositeSearchQuery : IRequest<List<SearchResultItem>>
    {
        public string SearchTerm { get; set; } = string.Empty;
        public Guid? CurrentUserId { get; set; }
    }
}
