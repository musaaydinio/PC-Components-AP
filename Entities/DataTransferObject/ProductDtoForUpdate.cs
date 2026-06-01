using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObject
{
    public record class ProductDtoForUpdate(int Id,String Name,decimal Price,int StockQuantity);

}
