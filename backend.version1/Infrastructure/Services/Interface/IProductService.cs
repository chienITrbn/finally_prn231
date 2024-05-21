using backend.version1.Domain.DTO;
using backend.version1.Infrastructure.Common.Paginate;
using Domain.Entities;

namespace backend.version1.Infrastructure.Services.Interface
{
    public interface IProductService : IBaseService<Product, ProductDto>
    {
        Task<PagedResponse<List<Product>>> GetAllProducts(PaginationFilter validFilter, IUriService uriService, string route);

        Task<ProductDto> GetProductById(int id);
    }
}