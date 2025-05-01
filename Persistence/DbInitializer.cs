using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;
using Persistence.Identity;
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
        private readonly StoreIdentityDbContext _identityDbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public DbInitializer( StoreDbContext context,StoreIdentityDbContext  identityDbContext,RoleManager<IdentityRole> roleManager,UserManager<User> userManager)
        {
            _context = context;
            _identityDbContext = identityDbContext;
           _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task InitializeIdentityAsync()

        { 
            if(_context.Database.GetPendingMigrations().Any())
             await _identityDbContext.Database.MigrateAsync(); 
            
            
            if (!_roleManager.Roles.Any()) 
            {await _roleManager.CreateAsync(new IdentityRole("Admin"));
                         
               await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));

            }
            if (!_userManager.Users.Any())
            {
                var superAdminUser = new User
                {
                    DisplayName = "Super Admin",
                    Email = "SuperAdmin@gmail.com",
                    UserName = "SuperAdmin",
                    PhoneNumber = "1234567890"


                };
                var adminUser = new User
                {
                    DisplayName = "Admin",
                    Email = "Admin@gmail.com",
                    UserName = "Admin",
                    PhoneNumber = "678907888"


                };
                


                await _userManager.CreateAsync(superAdminUser,"Passw0rd");
                await _userManager.CreateAsync(adminUser, "Passw0rd");


                await _userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
                await _userManager.AddToRoleAsync(adminUser, "Admin");


            }

        }

        public async Task InitializerAsync()
        {
            try
            {
               if(_context.Database.GetPendingMigrations().Any())
                 _context.Database.Migrate();
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
