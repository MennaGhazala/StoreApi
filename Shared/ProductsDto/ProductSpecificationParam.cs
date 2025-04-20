using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ProductsDto
{
    public class ProductSpecificationParam
    {
      public int? BrandId { get; set; }
      public int?TypeId { get; set; }

        public string? Search { get; set; }

        public ProductSort? Sort { get; set; }
    }
}
