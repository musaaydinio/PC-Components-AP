namespace Entities.DataTransferObject
{
    public record ProductDto()
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }

}
