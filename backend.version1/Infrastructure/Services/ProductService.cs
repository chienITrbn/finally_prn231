using backend.version1.Domain.AutoMapper;
using backend.version1.Domain.DTO;
using backend.version1.Domain.Interfaces;
using backend.version1.Infrastructure.Common.Paginate;
using backend.version1.Infrastructure.Services.Interface;
using Domain.Entities;

namespace backend.version1.Infrastructure.Services
{
    public class ProductService : BaseService<Product, ProductDto>, IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository, IAutoMapperBase<Product, ProductDto> mapper)
            : base(productRepository, mapper)
        {
            _productRepository = productRepository;
        }

        public async Task<PagedResponse<List<Product>>> GetAllProducts(PaginationFilter validFilter, IUriService uriService, string route)
        {
            var pagedProducts = await _productRepository.GetAllProduct(validFilter, uriService, route);
            return pagedProducts;
        }

        public async Task<ProductDto> GetProductById(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return _mapper.Map(product);
        }
    }
}