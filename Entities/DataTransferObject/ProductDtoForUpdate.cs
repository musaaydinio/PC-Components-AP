using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObject
{
    // Güncelleme işlemlerinde kullanmak üzere temel sınıfı genişlettik ve Id alanını zorunlu kıldık.
    public record class ProductDtoForUpdate : ProductDtoForManipulation
    {
        [Required]
        public int Id { get; set; }
    }

}
