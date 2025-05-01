using Domain.Contracts;

namespace StoreApi.Extensions
{
    public static class WebApplicationExtension
    {
       public static async Task<WebApplication> SeedDbAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbInttialzer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();

            await dbInttialzer.InitializerAsync();
            await dbInttialzer.InitializeIdentityAsync();
            return app;
        }
    }
}
