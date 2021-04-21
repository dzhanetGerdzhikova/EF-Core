using AutoMapper;
using ProductShop.DTO;
using ProductShop.Models;
using System.Linq;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            this.CreateMap<Product, ProductsInRangeDto>()
                .ForMember(pdto => pdto.SellerName, x => x.MapFrom(p => p.Seller.FirstName + " " + p.Seller.LastName));
              
            this.CreateMap<Product,ProductDto>()
                .ForMember(pdto=>pdto.BuyerFirstName,x=>x.MapFrom(p=>p.Buyer.FirstName))
                .ForMember(pdto=>pdto.BuyerLastName,x=>x.MapFrom(p=>p.Buyer.LastName));

            this.CreateMap<User, SoldProductsDto>()
                .ForMember(spdto => spdto.Products, x => x.MapFrom(u => u.ProductsSold.Where(p=>p.Buyer!=null)));

            this.CreateMap<Category, CategoryDto>()
                .ForMember(cdto => cdto.Name, x => x.MapFrom(c => c.Name))
                .ForMember(cdto => cdto.CountProduct, x => x.MapFrom(c => c.CategoryProducts.Count()))
                .ForMember(cdto => cdto.AvrPrice, x => x.MapFrom(c => c.CategoryProducts.Average(cp => cp.Product.Price).ToString("f2")))
                .ForMember(cdto => cdto.TotalSum, x => x.MapFrom(c => c.CategoryProducts.Sum(cp => cp.Product.Price).ToString("f2")));
        }
    }
}
