﻿using Shared;
using Shared.ProductsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction
{
    public interface IProductService
    {
        Task<PaginatedResult<ProductResultDto>> GetAllProductaAsync(ProductSpecificationParam specs );
        Task<ProductResultDto> GetProductaByIdAsync(int id);
        Task<IEnumerable<TypeResultDto>> GetAllTypesAsync();
        Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync();
    }
}
