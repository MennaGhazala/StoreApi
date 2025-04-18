using AutoMapper;
using Domain.Entities;
using Shared.ProductsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfile
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductResultDto>()
                .ForMember(d=>d.BrandName,option =>option.MapFrom(src=>src.productBrand.Name))
                .ForMember(d => d.TypeName, option => option.MapFrom(src => src.productType.Name))
                .ForMember(d=>d.PictureUrl,option=>option.MapFrom<PictureUrlResolver>());
            
            CreateMap<ProductBrand, BrandResultDto>();
            CreateMap<ProductType, TypeResultDto>();   
        }

    }
}
