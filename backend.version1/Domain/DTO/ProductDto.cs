using Domain.Entities;

namespace backend.version1.Domain.DTO
{
    public class ProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal FormattedPrice { get; set; }
        public Category Category { get; set; }
    }
}