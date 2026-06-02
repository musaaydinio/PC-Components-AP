namespace Entities.DataTransferObject
{
    public record ProductDto()
    {
        public int Id { get; init; }
        public required string Name { get; init; }
        public decimal Price { get; init; }
        public int StockQuantity { get; init; }
    }
}
