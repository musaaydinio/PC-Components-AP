using Entities.DataTransferObject;
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
        IEnumerable<ProductDto> GetAllProduct(bool trackChanges);
        ProductDto GetOneProductById(int id, bool trackChanges);
        ProductDto CreateOneProduct(ProductDtoForInsertion product);
        void UpdateOneProduct(int id,ProductDtoForUpdate productDto,bool trackChanges);
        void DeleteOneProduct(int id,bool trackChanges);

        (ProductDtoForUpdate productDtoForUpdate, Product product) GetOneProductForPatch(int id, bool trackChanges);
        void SaveChangesForPatch(ProductDtoForUpdate productDtoForUpdate,Product product);
    }
}
