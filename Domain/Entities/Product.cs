using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Product :BaseEntity<int>
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }

        public decimal Price { get; set; }
        public ProductBrand productBrand { get; set; }
        public int  BrandId { get; set; }

        public ProductType productType { get; set; }
        public int TypeId { get; set; }
    }
}
