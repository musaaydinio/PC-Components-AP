using System.ComponentModel.DataAnnotations;

namespace Entities.DataTranferObjcets
{
    // Kullanıcı girişi işlemlerinde kimlik doğrulama verilerini taşımak için kurduğumuz yapımız.
    public record UserForAuthenticationDto
    {
        [Required(ErrorMessage = "Usurname is required")]
        public string? UserName { get; init; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; init; }

    }
}
