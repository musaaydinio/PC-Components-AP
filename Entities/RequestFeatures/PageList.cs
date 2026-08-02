using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.RequestFeatures
{
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
                TotalPage = (int)Math.Ceiling (count/(double)pageeSize)
            };
            AddRange(items);
        }

        public static PageList<T> ToPagedList(IEnumerable<T> source,
            int pageNumber,int pageSize)
        {
            var count=source.Count();
            var items=source.Skip((pageNumber-1)* pageSize).Take(pageSize).ToList();
            
            return new PageList<T>(items,count,pageNumber,pageSize);
        }       
    }
}
