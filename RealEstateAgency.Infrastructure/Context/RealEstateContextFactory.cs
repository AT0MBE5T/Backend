using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace RealEstateAgency.Infrastructure.Context
{
    public class RealEstateContextFactory : IDesignTimeDbContextFactory<RealEstateContext>
    {
        public RealEstateContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddUserSecrets<RealEstateContextFactory>()
                .Build();
        
            var optionsBuilder = new DbContextOptionsBuilder<RealEstateContext>();
            optionsBuilder.UseNpgsql(config.GetConnectionString("RealEstateAgencyConnectionString"));
        
            return new RealEstateContext(optionsBuilder.Options);
        }
        
        // public RealEstateContext CreateDbContext(string[] args)
        // {
        //     var basePath = Directory.GetCurrentDirectory(); 
        //     
        //     if (basePath.Contains("Infrastructure"))
        //     {
        //         basePath = Path.Combine(basePath, "../RealEstateAgency.API");
        //     }
        //
        //     var config = new ConfigurationBuilder()
        //         .SetBasePath(basePath)
        //         .AddJsonFile("appsettings.json", optional: false)
        //         .AddUserSecrets("50b48331-3f6e-4d78-baaa-72d4ea4ced55")
        //         .AddEnvironmentVariables()
        //         .Build();
        //
        //     var connectionString = config.GetConnectionString("RealEstateAgencyConnectionString");
        //
        //     // ОТЛАДКА: Если строка пуста, выкинем понятную ошибку
        //     if (string.IsNullOrEmpty(connectionString))
        //     {
        //         throw new InvalidOperationException(
        //             $"Строка подключения не найдена! Искал в папке: {basePath}. " +
        //             $"Убедитесь, что в appsettings.json есть секция ConnectionStrings.");
        //     }
        //
        //     var optionsBuilder = new DbContextOptionsBuilder<RealEstateContext>();
        //     optionsBuilder.UseNpgsql(connectionString);
        //
        //     return new RealEstateContext(optionsBuilder.Options);
        // }
    }
}
