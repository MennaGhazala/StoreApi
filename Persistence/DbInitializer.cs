using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persistence
{
    public class DbInitializer : IDbInitializer
    {
        private readonly StoreDbContext _context;

        public DbInitializer( StoreDbContext context)
        {
            _context = context;
        }
        public async Task InitializerAsync()
        {
            try
            {
                //if(_context.Database.GetPendingMigrations().Any())
                // _context.Database.Migrate();
                if (!_context.ProductTypes.Any())
                {
                    var typeData = File.ReadAllText(@"..\Persistence\Data\Seeding\types.json");
                    var types = JsonSerializer.Deserialize<List<ProductType>>(typeData);
                    if (types is not null && types.Any())
                    {

                        await _context.ProductTypes.AddRangeAsync(types);
                        await _context.SaveChangesAsync();
                    }
                }
                if (!_context.ProductBrands.Any())
                {
                    var BrandsData = File.ReadAllText(@"..\Persistence\Data\Seeding\brands.json");
                    var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandsData);
                    if (Brands is not null && Brands.Any())
                    {

                     await   _context.ProductBrands.AddRangeAsync(Brands);
                    await    _context.SaveChangesAsync();
                    }
                }
                if (!_context.Products.Any())
                {
                    var ProductsData = File.ReadAllText(@"..\Persistence\Data\Seeding\products.json");
                    var Products = JsonSerializer.Deserialize<List<Product>>(ProductsData);
                    if (Products is not null && Products.Any())
                    {

                     await   _context.Products.AddRangeAsync(Products);
                     await   _context.SaveChangesAsync();
                    }
                }
            }
            catch { }
        }   
    }
}
