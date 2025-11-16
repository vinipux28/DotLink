using System.Collections.Generic;

namespace DotLink.Application.DTOs
{
    public class PaginatedResponse<T>
    {
        public int PageNumber { get; }
        public int PageSize { get; }
        public int TotalPages { get; }
        public int TotalCount { get; }
        public IEnumerable<T> Data { get; }

        public PaginatedResponse(IEnumerable<T> data, int count, int pageNumber, int pageSize)
        { 
            Data = data;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = count;
            TotalCount = count;
        }
    }
}