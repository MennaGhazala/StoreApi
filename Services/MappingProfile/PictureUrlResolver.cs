using AutoMapper;

using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Shared.ProductsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.MappingProfile
{


    public class PictureUrlResolver(IConfiguration  config) : IValueResolver<Product, ProductResultDto, string>

    {
        

        public string Resolve(Product source, ProductResultDto destination, string destMember, ResolutionContext context)
        {
            if(string.IsNullOrWhiteSpace(source.PictureUrl))
                return string.Empty;
            return $"{config["BaseUrl"]}{source.PictureUrl}";
        }
    }
}
