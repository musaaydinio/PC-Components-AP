using AutoMapper;
using Entities.DataTransferObject;
using Entities.Models;

namespace E_Ticaret_BitStore.Unitities.AutoMapper
{
    public class MappingProfilecs:Profile
    {
        public MappingProfilecs()
        {
            CreateMap<ProductDtoForUpdate, Product>();
            CreateMap<Product,ProductDto>();
        }
    }
}
