using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    // Veri Şekillendirme işlemlerini yaparak istemciye sadece talep ettiği alanları (property) dönmemizi sağlayan arayüzümüz.
    public interface IDataShaper<T>
    {
        IEnumerable<ExpandoObject> ShapeData(IEnumerable<T> enteties,string fieldsString);
        ExpandoObject ShapeData(T enteties,string fieldsString);
    }
}
