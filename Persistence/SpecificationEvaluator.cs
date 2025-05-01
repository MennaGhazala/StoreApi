using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<T> GetQuery<T>(IQueryable<T> baseQuery, Specification<T> specification) where T : class
          {
            var query = baseQuery;
            if(specification.Criteria is not null)
                query = query.Where(specification.Criteria);
            query = specification.Includes.Aggregate(query, (currentQuery, include) => currentQuery.Include(include));
            if (specification.OrderBy is not null)
                query = query.OrderBy(specification.OrderBy);
            else if (specification.OrderByDescending is not null)
               query = query.OrderByDescending(specification.OrderByDescending);
            
            if(specification.IsPaginted)
                query=query.Skip(specification.Skip).Take(specification.Take);
            
            return query;
          }
    }
}
