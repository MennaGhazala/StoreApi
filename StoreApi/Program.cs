
using AutoMapper;
using Domain.Contracts;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Persistence;
using Persistence.Data;
using Persistence.Identity;
using Persistence.Repositorys;
using Services;
using Services.Abstraction;
using Services.MappingProfile;
using Shared.ProductsDto;
using StackExchange.Redis;
using StoreApi.Factories;
using StoreApi.Middlewares;
using System.Reflection.Metadata;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace StoreApi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            }); 
           builder.Services.AddDbContext<StoreDbContext>(option =>
           {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
           });

            builder.Services.AddDbContext<StoreIdentityDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("IdentitySqlConnection"));
            });

            builder.Services.AddIdentity<User, IdentityRole>(
                option =>
                {
                    option.Password.RequireNonAlphanumeric = false;
                    option.Password.RequireUppercase = false;
                     option.Password.RequireLowercase = false;
                     option.Password.RequiredLength = 8;
                     option.Password.RequireDigit = true;
                    option.User.RequireUniqueEmail = true;
                }).AddEntityFrameworkStores<StoreIdentityDbContext>();         

            builder.Services.AddScoped<IDbInitializer,DbInitializer>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IServiceManager, ServiceManager>();
            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.AddAutoMapper(x=>x.AddProfile(new ProductProfile()));
            builder.Services.AddAutoMapper(typeof(BasketProfile));

            builder.Services.AddTransient<PictureUrlResolver>();
            builder.Services.AddSingleton<IConnectionMultiplexer>(
                _=> ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("Redis"))
                );

            builder.Services.Configure<ApiBehaviorOptions>( options=>
            {
                options.InvalidModelStateResponseFactory = ApiResponseFactory.CustomValidationErrorResponse;
                
         
            } );
            
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            await    SeedDbAsync(app);
            app.UseMiddleware<GlobalErrorHandlingMiddleware>();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();

            app.MapControllers();

            app.Run();
        }

       static async Task SeedDbAsync( WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbInttialzer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
         
            await dbInttialzer.InitializerAsync();
            await dbInttialzer.InitializeIdentityAsync();

        }
    }
}
