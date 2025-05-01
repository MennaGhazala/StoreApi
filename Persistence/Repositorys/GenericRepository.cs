using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositorys
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly StoreDbContext _context;

        public GenericRepository(StoreDbContext  context)
        {
           _context = context;
        }
        public async Task AddAsync(TEntity entity)
       =>await _context.Set<TEntity>().AddAsync(entity);

        public void DeleteAsync(TEntity entity)
        => _context.Set<TEntity>().Remove(entity);

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool isTrackable = false)
        {
            
            if (isTrackable) 
                return await _context.Set<TEntity>().ToListAsync();
            return await _context.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<TEntity?>> GetAllAsync(Specification<TEntity> specification)
        {
           return await ApplySpecification(specification).ToListAsync();
        }
        public async Task<TEntity?> GetAsync(Specification<TEntity> specification)
        {
            return await ApplySpecification(specification).FirstOrDefaultAsync();
        }

        private IQueryable<TEntity> ApplySpecification(Specification<TEntity> specification)
        => SpecificationEvaluator.GetQuery(_context.Set<TEntity>(), specification);
        public async Task<TEntity?> GetAsync(TKey id)
        => await _context.Set<TEntity>().FindAsync(id);

       

        public void UpdateAsync(TEntity entity)
        => _context.Set<TEntity>().Update(entity);

        public async Task<int> CountAsync(Specification<TEntity> specification)
        => await ApplySpecification(specification).CountAsync();
    }
}
