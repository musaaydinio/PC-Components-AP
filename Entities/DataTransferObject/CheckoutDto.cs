namespace Entities.DataTransferObject
{
    public record CheckoutDto(
    string CardHolderName,
    string CardNumber,
    string ExpirationDate,
    string Cvc
);
}
