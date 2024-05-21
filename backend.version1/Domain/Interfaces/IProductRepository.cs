using backend.Domain.Interfaces;
using backend.version1.Infrastructure.Common.Paginate;
using backend.version1.Infrastructure.Services.Interface;
using Domain.Entities;

namespace backend.version1.Domain.Interfaces
{
    public interface IProductRepository : IRepositoryBase<Product>
    {
        public Task<PagedResponse<List<Product>>> GetAllProduct(PaginationFilter validFilter, IUriService uriService, string route);
    }
}