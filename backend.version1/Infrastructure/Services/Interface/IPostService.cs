using backend.version1.Domain.DTO;
using backend.version1.Domain.Entities;
using backend.version1.Infrastructure.Common.Paginate;

namespace backend.version1.Infrastructure.Services.Interface
{
    public interface IPostService : IBaseService<Post, PostDto>
    {
        public Task<PagedResponse<List<Post>>> GetAllPost(PaginationFilter validFilter, IUriService uriService, string route);
    }
}