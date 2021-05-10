using AutoMapper;
using Data.Entities;
using Model.DTOs;

namespace Model.Configurations
{
    public class MapperInitilizer : Profile
    {
        public MapperInitilizer()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CreateCategoryDto>().ReverseMap();
            CreateMap<Condition, ConditionDto>().ReverseMap();
            CreateMap<Condition, CreateConditionDto>().ReverseMap();
            CreateMap<Demand, DemandDto>().ReverseMap();
            CreateMap<Demand, CreateDemandDto>().ReverseMap();
            CreateMap<Trademark, TrademarkDto>().ReverseMap();
            CreateMap<Trademark, CreateTrademarkDto>().ReverseMap();
            CreateMap<Vendor, VendorDto>().ReverseMap();
            CreateMap<Vendor, CreateVendorDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, CreateProductDto>().ReverseMap();
            CreateMap<ProductImage, ProductImageDto>().ReverseMap();
            CreateMap<ProductImage, CreateProductImageDto>().ReverseMap();
            CreateMap<ShoppingCart, CartDto>().ReverseMap();
            CreateMap<WishList, WishlistDto>().ReverseMap();
            CreateMap<AppUser, UserDto>().ReverseMap();
            CreateMap<AppUser, CreateUserDto>().ReverseMap();
            CreateMap<UpdateUserDto, AppUser>().ReverseMap();
        }
    }
}
