using Entities.DataTransferObject;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository;
using Story.EF_Core;

namespace Presentation.Controllers
{
    // Sipariş ve ödeme işlemlerini yönettiğimiz yer
    [ApiController]
    [Route("api/orders")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly StoreDbcontex _context;

        public OrdersController(StoreDbcontex context)
        {
            _context = context;
        }

        // Ödemeyi alan, stoğu kontrol edip düşüren ve siparişi oluşturan istek
        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] CheckoutDto dto)
        {
            var username = User.Identity?.Name ?? "Anonymous";

            // Kullanıcının sepetindeki ürünleri veritabanından getiriyoruz
            var cartItems = await _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.Username == username)
                .ToListAsync();

            // Sepet boşsa ödeme yapılmasına izin vermiyoruz
            if (!cartItems.Any())
                return BadRequest(new { message = "Your cart is empty! You need to add items to your cart before you can pay.." });

            // Stok Kontrolü: Sepetteki ürün miktarı depodaki stoktan fazla mı bakıyoruz
            foreach (var item in cartItems)
            {
                if (item.Product != null && item.Product.StockQuantity < item.Quantity)
                {
                    return BadRequest(new
                    {
                        message = $"Payment Failed: '{item.Product.Name}' there is not enough stock for this product! Available Stock: {item.Product.StockQuantity}"
                    });
                }
            }

            // Ödeme Simülasyonu: Test için 5 ile başlayan kartlarda bakiye yetersiz hatası döndürüyoruz
            if (dto.CardNumber.StartsWith("5"))
            {
                return BadRequest(new { message = "Payment Failed: Insufficient balance on bank card!" });
            }

            // Stok Düşürme: Sepette kaç tane ürün alındıysa veritabanındaki stoktan o kadar düşüyoruz
            foreach (var item in cartItems)
            {
                if (item.Product != null)
                {
                    item.Product.StockQuantity -= item.Quantity;
                }
            }

            // Ödenmesi gereken toplam tutarı hesaplıyoruz
            var grandTotal = cartItems.Sum(c => (c.Product?.Price ?? 0) * c.Quantity);

            // 5. Sipariş kaydını oluşturuyoruz
            var order = new Order
            {
                Username = username,
                TotalAmount = grandTotal,
                OrderDate = DateTime.UtcNow,
                OrderStatus = "Paid & Completed"
            };

            _context.Orders.Add(order);

            //  Ödeme bittiği için kullanıcının sepetini temizliyoruz
            _context.CartItems.RemoveRange(cartItems);

            // Stok düşümünü, yeni siparişi ve sepet temizliğini tek seferde veritabanına kaydediyoruz
            await _context.SaveChangesAsync();

            // Kullanıcıya sipariş numarasını ve başarılı mesajını döndürüyoruz
            return Ok(new
            {
                message = "Payment successful! Your order has been received.",
                orderId = order.OrderId,
                totalPaid = grandTotal,
                status = order.OrderStatus
            });
        }
    }
}