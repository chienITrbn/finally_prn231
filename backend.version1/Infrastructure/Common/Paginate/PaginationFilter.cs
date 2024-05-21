using backend.version1.Common.Const;

namespace backend.version1.Infrastructure.Common.Paginate
{
    public class PaginationFilter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public PaginationFilter()
        {
            this.PageNumber = Config.PAGE_NUMBER;
            this.PageSize = Config.DEFAULT_PERPAGE;
        }

        public PaginationFilter(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber < 1 ? Config.PAGE_NUMBER : pageNumber;
            this.PageSize = pageSize > Config.DEFAULT_PERPAGE ? Config.DEFAULT_PERPAGE : pageSize;
        }
    }
}