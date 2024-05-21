using backend.Common;
using backend.version1.Domain.DTO;
using backend.version1.Infrastructure.Common.Paginate;
using backend.version1.Infrastructure.Services.Interface;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace backend.version1.Controllers
{
    public class ProductController : ApiBaseController<Product, ProductDto>
    {
        private readonly IProductService _service;
        private readonly IUriService _uriService;

        public ProductController(IProductService service, ILogger<ApiBaseController<Product, ProductDto>> logger, IUriService uriService) : base(service, logger)
        {
            _service = service;
            _uriService = uriService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts([FromQuery] PaginationFilter filter)
        {
            try
            {
                var route = Request.Path.Value;
                var products = await _service.GetAllProducts(filter, _uriService, route);
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetAllProducts");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var product = await _service.GetProductById(id);
                if (product == null)
                {
                    return NotFound(new ErrorResponse<ProductDto>(new string[] { "Product not found" }));
                }

                return Ok(new SuccessResponse<ProductDto>(product, string.Empty));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}