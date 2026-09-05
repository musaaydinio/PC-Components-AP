namespace Entities.DataTransferObject
{
    // İstemciden sepete ürün ekleme isteği atılırken alınan veri modeli.
    public record AddToCartDto(int ProductId, int Quantity);
}
