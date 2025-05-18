using AutoMapper;

using Domain.Entities.OrderEntities;
using Shared;
using Shared.OrderDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfile
{
    public class OrderProfile:Profile
    {
        public OrderProfile() 
        {
            CreateMap<Address,Shared.OrderDto.AddressDto>().ReverseMap();

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductId, options => options.MapFrom(scr => scr.Product.ProductId))
                .ForMember(dest => dest.ProductName, options => options.MapFrom(scr => scr.Product.ProductName))
                .ForMember(dest => dest.PictureUrl, options => options.MapFrom(scr => scr.Product.PictureUrl));

            CreateMap<Order, OrderResult>()
                .ForMember(dest => dest.PaymentStatus, options => options.MapFrom(scr => scr.PaymentStatus.ToString()))
                .ForMember(dest => dest.DeliveryMethod, options => options.MapFrom(scr => scr.DeliveryMethod.ShortName))
                .ForMember(dest => dest.Total, options => options.MapFrom(scr => scr.SubTotal+scr.DeliveryMethod.Price));

            CreateMap<DeliveryMethod, DeliveryMethodResult>();

            
        }   
       
    }
}
