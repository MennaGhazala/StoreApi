using Domain.Contracts;
using Domain.Entities;
using Shared.ProductsDto;

namespace Services.Specifications
{
    public class ProductWithFilterSpecification : Specification<Product>
    {
        public ProductWithFilterSpecification(int id)
            : base(product => product.Id == id)
        {
            AddInclude(product => product.productBrand);
            AddInclude(product => product.productType);
        }
        public ProductWithFilterSpecification(ProductSpecificationParam specs)
           : base(product => (!specs.BrandId.HasValue || product.BrandId == specs.BrandId) &&
                           (!specs.TypeId.HasValue || product.TypeId == specs.TypeId) &&
                           (string.IsNullOrWhiteSpace(specs.Search) ||
           product.Name.ToLower().Contains(specs.Search.ToLower().Trim()))
           )
        { 
            AddInclude(product => product.productBrand);
            AddInclude(product => product.productType);

            ApplyPagintion(specs.PageIndex, specs.PageSize);
            if (specs.Sort is not null)
            {
                switch (specs.Sort)
                {
                    case ProductSort.NameAsc:
                        SetOrderBy(product => product.Name);
                        break;

                    case ProductSort.NameDesc:
                        SetOrderByDescending(product => product.Name);
                        break;

                    case ProductSort.PriceAsc:
                        SetOrderBy(product => product.Price);
                        break;

                    case ProductSort.PriceDesc:
                        SetOrderByDescending(product => product.Price);
                        break;

                    default:
                        SetOrderBy(product => product.Id);
                        break;
                



                }
            }

        }
    }
}
