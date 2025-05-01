
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
using Shared.IdentityDto;
using Shared.ProductsDto;
using StackExchange.Redis;
using StoreApi.Extensions;
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

            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddCoreServices(builder.Configuration);
            builder.Services.AddPresentationServices();


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
           
            var app = builder.Build();

             await app.SeedDbAsync();

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

      
    }
}
