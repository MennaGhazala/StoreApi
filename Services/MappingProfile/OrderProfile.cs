using AutoMapper;
using Domain.Entities.Identity;
using Shared;
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
            CreateMap<Address,AddressDto>().ReverseMap();
        }   
       
    }
}
