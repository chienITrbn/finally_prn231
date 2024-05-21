using backend.Common;

namespace backend.version1.Infrastructure.Common.Paginate
{
    public class PagedResponse<T> : BaseResponse<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public Uri FirstPage { get; set; }
        public Uri LastPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public Uri NextPage { get; set; }
        public Uri PreviousPage { get; set; }

        public PagedResponse(T data, int pageNumber, int pageSize)
            : base(data, true, string.Empty, Array.Empty<string>(), 200)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            Data = data;
            Message = string.Empty;
            Succeeded = true;
            Errors = Array.Empty<string>();
        }
    }
}