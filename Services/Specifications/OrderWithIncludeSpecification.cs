using Domain.Contracts;
using Domain.Entities.OrderEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.Specifications
{
    public class OrderWithIncludeSpecification
        : Specification<Order>
    {
        public OrderWithIncludeSpecification(Guid id) 
            : base(order =>order.Id==id)
        {
            AddInclude(x => x.OrderItems);
            AddInclude(y => y.DeliveryMethod);
        }

        public OrderWithIncludeSpecification(string email)
           : base(order => order.BuyEmail == email)
        {
            AddInclude(x => x.OrderItems);
            AddInclude(y => y.DeliveryMethod);
            SetOrderBy(x => x.OrderDate);

        }
    }
}
