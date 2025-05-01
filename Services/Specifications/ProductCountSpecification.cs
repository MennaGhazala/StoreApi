using Domain.Contracts;
using Domain.Entities;
using Shared.ProductsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    public class ProductCountSpecification: Specification<Product>
    {
        public ProductCountSpecification(ProductSpecificationParam specs)
           : base(product => (!specs.BrandId.HasValue || product.BrandId == specs.BrandId) &&
                           (!specs.TypeId.HasValue || product.TypeId == specs.TypeId) &&
                           (string.IsNullOrWhiteSpace(specs.Search) ||
           product.Name.ToLower().Contains(specs.Search.ToLower().Trim()))
           )
        { 

        }
    }
}
