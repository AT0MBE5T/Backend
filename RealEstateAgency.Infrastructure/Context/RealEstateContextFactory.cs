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
    }
}
