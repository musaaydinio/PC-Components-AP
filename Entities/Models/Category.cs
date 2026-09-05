using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    // E-ticaret sistemimizdeki ürün kategorilerini temsil ettiğimiz temel modelimiz.
    public class Category
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
