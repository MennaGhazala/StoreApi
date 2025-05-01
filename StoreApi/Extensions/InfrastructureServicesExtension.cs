using Domain.Contracts;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using Persistence.Data;
using Persistence.Identity;
using Persistence.Repositorys;
using Shared.IdentityDto;
using StackExchange.Redis;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;

namespace StoreApi.Extensions
{
    public static class InfrastructureServicesExtension
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            
            services.AddDbContext<StoreDbContext>(option =>
            {
                option.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddDbContext<StoreIdentityDbContext>(option =>
            {
                option.UseSqlServer(configuration.GetConnectionString("IdentitySqlConnection"));
            });
          services.AddSingleton<IConnectionMultiplexer>(
               _ => ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis"))
               );
        
            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IBasketRepository, BasketRepository>();

            services.ConfigureIdentity();
            services.ConfigureJwt(configuration);
        return services;
        }
        private static IServiceCollection ConfigureIdentity (this IServiceCollection services) 
        {
            services.AddIdentity<User, IdentityRole>(
               option =>
               {
                   option.Password.RequireNonAlphanumeric = false;
                   option.Password.RequireUppercase = false;
                   option.Password.RequireLowercase = false;
                   option.Password.RequiredLength = 8;
                   option.Password.RequireDigit = true;
                   option.User.RequireUniqueEmail = true;
               }).AddEntityFrameworkStores<StoreIdentityDbContext>();
            return services;
           
        }

        private static IServiceCollection ConfigureJwt(this IServiceCollection  services, IConfiguration configuration)
        {
            var jwtConfig =configuration.GetSection("JwtOptions").Get<JwtOptions>();

            services.AddAuthentication(option =>
            {
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(option =>
            {
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = jwtConfig.Audience,
                    ValidIssuer = jwtConfig.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecurityKey))

                };
            });
        return services;
        }



    }
}
