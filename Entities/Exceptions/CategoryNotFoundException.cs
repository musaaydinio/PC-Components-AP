namespace Entities.Exceptions
{
    // Aranan ID'ye sahip kategori veritabanında bulunamadığında fırlattığımız, miras alınmasını engellediğimiz özelleştirilmiş 404 hata sınıfımız.
    public sealed class CategoryNotFoundException
       : NotFoundException
    {
        public CategoryNotFoundException(int id)
            : base($"Category with id : {id} could not found.")
        {

        }
    }
}
