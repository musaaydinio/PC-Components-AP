namespace Entities.Exceptions
{
    // Aranan ID'ye sahip ürün veritabanında bulunamadığında fırlattığımız, miras alınmasını engellediğimiz özelleştirilmiş 404 hata sınıfımız.
    public sealed class ProductNotFoundException : NotFoundException
    {
        public ProductNotFoundException(int id) : base($"the product with id :{id} could not found.")
        {

        }
    }
}
