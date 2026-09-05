using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    // ASP.NET Core Identity altyapısını genişleterek kendi özel kullanıcı alanlarımızı eklediğimiz modelimiz.
    public class User : IdentityUser
    {
        public String? FistName { get; set; }
        public String? LastName { get; set; }
        public String? RefreshToken { get; set; }
        public DateTime RefreshTokenExpriyTime { get; set; }
    }
}
