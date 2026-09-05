namespace Entities.DataTransferObject
{
    // Sepetteki tek bir ürün, birim fiyat, miktar ve satır toplamı detayını temsil eden DTO.
    public record CartItemDto(int ProductId, string ProductName, decimal Price, int Quantity, decimal TotalPrice);

    // Kullanıcının aktif sepet özetini ve hesaplanan genel toplam tutarını sunan DTO.
    public record CartSummaryDto(List<CartItemDto> Items, decimal GrandTotal);
}
