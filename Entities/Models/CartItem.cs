namespace Entities.Models
{
    // SQL Server tarafındaki CartItems tablosunu temsil eden Entity sınıfı.
    // Kullanıcıların sepetindeki ürünleri veritabanında kalıcı olarak tutar.
    public class CartItem
    {
        public int CartItemId { get; set; }
        public string Username { get; set; } = string.Empty; // Token'dan gelen kullanıcı
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        public int Quantity { get; set; }
    }
}
