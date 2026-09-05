using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    // Tüm parametre sınıflarımız için temel özellikleri tanımlıyoruz.
    public abstract class RequestParameters
    {
        const int maxPageSize = 50;
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;

        // İstemciden gelen sayfa boyutunu kontrol edip, belirlediğimiz maksimum sınırı aşmasını engelliyoruz.
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value > maxPageSize ? maxPageSize : value; }
        }

        public String? OrderBy { get; set; }
        public String? Fields { get; set; }
    }
}
