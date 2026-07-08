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
        Task<IEnumerable<ProductDto>>GetAllProductAsync(bool trackChanges);
        Task<ProductDto>GetOneProductByIdAsync(int id, bool trackChanges);
        Task<ProductDto> CreateOneProductAsync(ProductDtoForInsertion product);
        Task UpdateOneProductAsync(int id,ProductDtoForUpdate productDto,bool trackChanges);
        Task DeleteOneProductAsync(int id,bool trackChanges);

        Task<(ProductDtoForUpdate productDtoForUpdate, Product product)>GetOneProductForPatchAsync(int id, bool trackChanges);
        Task SaveChangesForPatchAsync(ProductDtoForUpdate productDtoForUpdate,Product product);
    }
}
