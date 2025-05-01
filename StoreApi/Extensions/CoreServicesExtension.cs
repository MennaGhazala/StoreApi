using Services.Abstraction;
using Services;
using Shared.IdentityDto;

namespace StoreApi.Extensions
{
    public static  class CoreServicesExtension
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IServiceManager, ServiceManager>();
            services.AddAutoMapper(typeof(Services.ServiceManager).Assembly);
            services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));




            return services;
        }
    }
}
