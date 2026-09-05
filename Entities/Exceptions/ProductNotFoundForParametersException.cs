namespace Entities.Exceptions
{
    public class ProductNotFoundForParametersException : NotFoundException
    {
        public ProductNotFoundForParametersException(int pageNumber)
            : base($"No products found on page {pageNumber}. The requested page is empty or out of range.")
        {
        }
    }
}
