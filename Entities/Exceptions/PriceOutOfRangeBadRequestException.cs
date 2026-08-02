namespace Entities.Exceptions
{
    public class PriceOutOfRangeBadRequestException : BadRequestException
    {
        public PriceOutOfRangeBadRequestException()
            : base("Maximum price should be less than 150000 and greater tahn 10.")
        {
            
        }
    }
}
