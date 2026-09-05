namespace Entities.DataTransferObject
{
    // Kategori detaylarını ve bu kategori altında yer alan ürünleri 
    // API istemcisine sunmak için kullanılan DTO.
    public record CategoryDto
    {
        public int CategoryId { get; init; }
        public string? CategoryName { get; init; }
        public ICollection<ProductDto>? Products { get; init; }
    }
}
