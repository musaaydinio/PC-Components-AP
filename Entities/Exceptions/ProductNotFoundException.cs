namespace Entities.Exceptions
{
    public sealed class ProductNotFoundException : NotFoundException
    {
        public ProductNotFoundException(int id) : base($"the product with id :{id} could not found.")
        {

        }
    }
}
