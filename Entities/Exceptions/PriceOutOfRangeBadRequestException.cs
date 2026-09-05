namespace Entities.Exceptions
{
    // Ürün fiyatı iş kurallarımıza uymadığında fırlattığımız 400 (Bad Request) hata sınıfımız.
    public class PriceOutOfRangeBadRequestException : BadRequestException
    {
        public PriceOutOfRangeBadRequestException()
            : base("Maximum price should be less than 150000 and greater tahn 100.")
        {
            
        }
    }
}
