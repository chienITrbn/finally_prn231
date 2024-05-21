using backend.Domain.Interfaces;
using backend.version1.Domain.Entities;
using backend.version1.Infrastructure.Common.Paginate;
using backend.version1.Infrastructure.Services.Interface;

namespace backend.version1.Domain.Interfaces
{
    public interface IPostRepository : IRepositoryBase<Post>
    {
        public Task<PagedResponse<List<Post>>> GetAllPost(PaginationFilter validFilter, IUriService uriService, string route);
    }
}