namespace Entities.RequestFeatures
{
    public class ProductParameters : RequestParameters
    {
        // İstemciden ürünlerle ilgili filtreleme, arama ve sıralama parametrelerini aldığımız sınıfımız.
        public uint MinPrice { get; set; } = 0;
        public uint MaxPrice { get; set; } = uint.MaxValue;

        // İstemcinin girdiği fiyat aralığının mantıklı olup olmadığını doğruluyoruz.
        public bool ValidPriceRange => MaxPrice>=MinPrice;

        public String? SearchTerm { get; set; }

        public ProductParameters()
        {
            // İstemci özel bir sıralama belirtmezse, varsayılan olarak ID'ye göre sıralıyoruz.
            OrderBy = "id";
        }
    }
}