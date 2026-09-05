namespace Entities.Models
{
    // Ödeme simülasyonu başarıyla tamamlanan siparişlerin kayıtlarını tutar.
    public class Order
    {
        public int OrderId { get; set; }
        public string Username { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public string OrderStatus { get; set; } = "Completed";
    }
}
