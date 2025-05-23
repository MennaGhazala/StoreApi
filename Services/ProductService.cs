﻿using AutoMapper;
using Domain.Contracts;
using Domain.Entities;
using Services.Abstraction;
using Shared.ProductsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Services.Specifications;
using Shared;
using Domain.Exceptions;

namespace Services
{
    public class ProductService(IUnitOfWork unitOfWork, IMapper mapper) : IProductService
    {
        public async Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync()
        {
            var brands= await unitOfWork.GetRepository<ProductBrand,int >().GetAllAsync();
             var mappedBrand=mapper.Map<IEnumerable<BrandResultDto>>(brands);  
        
            return mappedBrand;
        
        }

        public async Task<PaginatedResult<ProductResultDto>> GetAllProductaAsync(ProductSpecificationParam  specificationParam)
        {
            var specs = new ProductWithFilterSpecification(specificationParam);
            var products = await unitOfWork.GetRepository<Product, int>().GetAllAsync(specs);
            var countSpecs = new ProductCountSpecification(specificationParam);
            var totalCount = await unitOfWork.GetRepository<Product, int>().CountAsync(countSpecs);
            var mappedproducts= mapper.Map<IEnumerable<ProductResultDto>>(products);
            return new PaginatedResult<ProductResultDto>
                    (specificationParam.PageIndex, specificationParam.PageSize,totalCount, mappedproducts);
        }

        public async Task<IEnumerable<TypeResultDto>> GetAllTypesAsync()
        {
            var Types = await unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            
            var mappedType = mapper.Map<IEnumerable<TypeResultDto>>(Types);

            return mappedType;
        }

        public  async Task<ProductResultDto> GetProductaByIdAsync(int id)
        {
           
            var specs = new ProductWithFilterSpecification( id);

            var product = await unitOfWork.GetRepository<Product, int>().GetAsync(specs);
            


            return product is null ? throw new ProductNotFoundException(id) : mapper.Map<ProductResultDto>(product);

        }
    }

   
}
