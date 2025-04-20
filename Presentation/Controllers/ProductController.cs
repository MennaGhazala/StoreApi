using Microsoft.AspNetCore.Mvc;
using Services.Abstraction;
using Shared.ProductsDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class ProductController(IServiceManager serviceManager) : ApiController
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResultDto>>> GetAllProducts([FromQuery] ProductSpecificationParam specs)
        {
            var products= await serviceManager.ProductService.GetAllProductaAsync(specs);
            return Ok(products);

        }
        [HttpGet]
        public async Task <ActionResult<ProductResultDto>>GetProduct(int id) 
        {
        var product = await serviceManager.ProductService.GetProductaByIdAsync(id);
            return Ok(product);
        
        
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BrandResultDto>>> GetAllBrands()
        {
            var brands = await serviceManager.ProductService.GetAllBrandsAsync();
            return Ok(brands);

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypeResultDto>>> GetAllTypes()
        {
            var types = await serviceManager.ProductService.GetAllTypesAsync();
            return Ok(types);

        }



    }
}
