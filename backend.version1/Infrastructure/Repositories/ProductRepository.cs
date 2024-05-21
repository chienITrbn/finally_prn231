using backend.Infrastructure.Repositories;
using backend.version1.Domain.Interfaces;
using backend.version1.Helpers;
using backend.version1.Infrastructure.Common.Paginate;
using backend.version1.Infrastructure.Services.Interface;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.version1.Infrastructure.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }

        public async Task<PagedResponse<List<Product>>> GetAllProduct(PaginationFilter validFilter, IUriService uriService, string route)
        {
            var products = await ApplicationDbContext.Products.ToListAsync();

            var totalRecords = products.Count;
            var pagedData = products
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToList();

            return PaginationHelper.CreatePagedReponse(pagedData, validFilter, totalRecords, uriService, route);
        }
    }
}