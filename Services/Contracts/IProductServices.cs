using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IProductServices
    {
        IEnumerable<Product> GetAllProduct(bool trackChanges);
        Product GetOneProductById(int id, bool trackChanges);
        Product CreateOneProduct(Product product);
        void UpdateOneProduct(int id, Product product,bool trackChanges);
        void DeleteOneProduct(int id,bool trackChanges);
    }
}
