using Entities.DataTransferObject;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Story.EF_Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    // Sepetle ilgili tüm istekleri burada karşılıyoruz.
    [ApiController]
    [Route("api/cart")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly StoreDbcontex _context;

        // Veritabanı bağlantımızı içeri alıyoruz
        public CartController(StoreDbcontex context)
        {
            _context = context;
        }

        // Kullanıcının sepetindeki ürünleri ve toplam tutarı getiren istek
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            // Giriş yapan kullanıcının adını Token içinden çekiyoruz
            var username = this.User.Identity?.Name ?? "Anonymous";

            var cartItems = await _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.Username == username)
                .ToListAsync();

            // Çektiğimiz verileri ekrana düzgün basabilmek için DTO formatına çeviriyoruz ve her ürünün toplam fiyatını hesaplıyoruz
            var itemDtos = cartItems.Select(ci => new CartItemDto(
                ci.ProductId,
                ci.Product?.Name ?? "Unknown product.",
                ci.Product?.Price ?? 0,
                ci.Quantity,
                (ci.Product?.Price ?? 0) * ci.Quantity
            )).ToList();

            // Sepetteki tüm ürünlerin genel toplam tutarını hesaplıyoruz
            var grandTotal = itemDtos.Sum(i => i.TotalPrice);

            // Ürün listesini ve genel toplam fiyatı geri döndürüyoruz
            return Ok(new CartSummaryDto(itemDtos, grandTotal));
        }

        // Sepete yeni bir ürün ekleyen ya da var olan ürünün adedini artıran istek
        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDto dto)
        {
            var username = User.Identity?.Name ?? "Anonymous";

            // Eklenmek istenen ürün gerçekten veritabanında var mı bakıyoruz
            var product =await _context.Products.FindAsync(dto.ProductId);

            if (product == null) return NotFound("Product not found.");

            var existingItem=await _context.CartItems.FirstOrDefaultAsync(
                c=>c.Username == username && c.ProductId==dto.ProductId);

            if(existingItem != null)
            {
                // Ürün zaten sepette varsa sıfırdan eklemek yerine sadece adedini artırıyoruz
                existingItem.Quantity += dto.Quantity;
            }
            else
            {
                // Ürün sepette hiç yoksa yeni bir sepet elemanı olarak ekliyoruz
                _context.CartItems.Add(new CartItem
                {
                    Username = username,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity
                });
            }
            // Yapılan değişiklikleri veritabanına kaydediyoruz
            await _context.SaveChangesAsync();
            return Ok(new {message= "Product added to cart." });
        }
        // Sepetten seçilen bir ürünü tamamen silen istek
        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var username = User.Identity?.Name ?? "Anonymous";
            // Silinecek ürünü kullanıcının sepetinde arıyoruz
            var item = await _context.CartItems
                .FirstOrDefaultAsync(c => c.Username == username && c.ProductId == productId);

            if (item == null) return NotFound("This product was not found in the cart.");
            // Ürünü sepetten kaldırıp veritabanını güncelliyoruz
            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();

            return Ok(new { message = "The item has been removed from the cart." });
        }
    }
}
