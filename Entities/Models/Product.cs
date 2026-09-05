using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    // Sistemimizdeki satılabilir ürünleri ve stok durumlarını tanımladığımız modelimiz.
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string? ImageUrl { get; set; } 
        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}

