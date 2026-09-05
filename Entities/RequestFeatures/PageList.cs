using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
    // Sayfalanmış verilerimizi ve sayfalama bilgilerini (MetaData) tek bir yapıda paketliyoruz.
    public class PageList<T> : List<T>
    {
        public MetaData MetaData { get; set; }
        public PageList(List<T> items,int count, int pageNumber,int pageeSize)
        {
            MetaData = new MetaData()
            {
                TotalCount = count,
                PageSize = pageeSize,
                CurrentPage=pageNumber,
                // Toplam sayfa sayısını matematiksel olarak hesaplıyoruz.
                TotalPage = (int)Math.Ceiling (count/(double)pageeSize)
            };
            AddRange(items);
        }

        // Koleksiyonu alıp, Skip ve Take metotlarıyla sayfalanmış bir listeye çeviriyoruz.
        public static PageList<T> ToPagedList(IEnumerable<T> source,
            int pageNumber,int pageSize)
        {
            var count=source.Count();
            var items=source.Skip((pageNumber-1)* pageSize).Take(pageSize).ToList();
            
            return new PageList<T>(items,count,pageNumber,pageSize);
        }       
    }
}
