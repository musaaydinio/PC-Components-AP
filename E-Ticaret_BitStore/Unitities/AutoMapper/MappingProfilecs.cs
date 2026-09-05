using AutoMapper;
using Entities.DataTransferObject;
using Entities.Models;

namespace E_Ticaret_BitStore.Unitities.AutoMapper
{
    // DTO nesneleri ile Entity modellerimiz arasındaki AutoMapper eşleme kurallarını belirlediğimiz profil sınıfımız.
    public class MappingProfilecs:Profile
    {
        public MappingProfilecs()
        {
            CreateMap<ProductDtoForUpdate, Product>().ReverseMap();
            CreateMap<Product,ProductDto>();
            CreateMap<ProductDtoForInsertion, Product>();
            CreateMap<UserForResgistrationDto, User>();
            CreateMap<Category, CategoryDto>();
        }
    }
}
