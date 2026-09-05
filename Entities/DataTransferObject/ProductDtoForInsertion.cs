using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObject
{
    // Yeni ürün eklerken kullandığımız bu yapıyı, DRY prensibi gereği temel sınıftan miras alarak oluşturduk.
    public record ProductDtoForInsertion : ProductDtoForManipulation
    {
        [Required(ErrorMessage = "CategoryId is required.")]
        public int CategoryId { get; init; }
    }
}
