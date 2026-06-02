using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObject
{
    public abstract record ProductDtoForManipulation
    {
        [Required(ErrorMessage = "Ürün adı boş geçilemez.")]
        [MinLength(2, ErrorMessage = "Ürün adı en az 2 karakter olmalıdır.")]
        [MaxLength(100, ErrorMessage = "Ürün adı 100 karakteri geçemez.")]
        public string Name { get; init; }

        [Required(ErrorMessage = "Fiyat bilgisi zorunludur.")]
        [Range(0.01, 500000, ErrorMessage = "Fiyat 0'dan büyük olmalıdır.")]
        public decimal Price { get; init; }

        [Required(ErrorMessage = "Stok bilgisi zorunludur.")]
        [Range(0, 10000, ErrorMessage = "Stok adedi eksi bir değer olamaz.")]
        public int StockQuantity { get; init; }
    }
}
