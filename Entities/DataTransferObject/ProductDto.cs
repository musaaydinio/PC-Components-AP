using Entities.Models;

namespace Entities.DataTransferObject
{
    // API'den dışarıya veri taşırken verinin yolda değişmemesi için record ve init yapısı kurduk.
    public record ProductDto()
    {
        public int Id { get; init; }
        public required string Name { get; init; }
        public decimal Price { get; init; }
        public int StockQuantity { get; init; }
    }
}
