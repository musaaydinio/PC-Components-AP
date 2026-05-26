namespace Entities.Exceptions
{
    public sealed class ProductNotFound : NotFoundException
    {
        public ProductNotFound(int id) : base($"the product with id :{id} could not found.")
        {

        }
    }
}
