using backend.version1.Infrastructure.Common.Paginate;

namespace backend.version1.Infrastructure.Services.Interface
{
    public interface IUriService
    {
        public Uri GetPageUri(PaginationFilter filter, string route);
    }
}