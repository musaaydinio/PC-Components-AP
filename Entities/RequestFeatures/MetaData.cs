using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    // Sayfalama (Pagination) işlemlerinde istemciye döneceğimiz meta veri bilgilerini tutuyoruz.
    public class MetaData
    {
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        // Önceki veya sonraki sayfaların olup olmadığını kontrol ediyoruz.
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext=> CurrentPage < TotalPage;
    }
}
