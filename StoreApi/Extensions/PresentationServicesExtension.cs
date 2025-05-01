using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using StoreApi.Factories;
using System.Text.Json.Serialization;

namespace StoreApi.Extensions
{
    public static class PresentationServicesExtension
    {
        public static IServiceCollection AddPresentationServices(this IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });



            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = ApiResponseFactory.CustomValidationErrorResponse;


            });

            services.ConfigureSwagger();

            return services;
        }

        public static IServiceCollection ConfigureSwagger (this IServiceCollection services) 
        {
            services.AddEndpointsApiExplorer();


            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });


                options.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter Bearer Token : Bearer {your token}"
                });




                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                             Reference = new OpenApiReference
                             {
                                Type = ReferenceType.SecurityScheme,
                                  Id = "Bearer"
                             }
                        },
                      new  string[] {}

                    }
                });
            });
                return services;

            
        }
    }
}
