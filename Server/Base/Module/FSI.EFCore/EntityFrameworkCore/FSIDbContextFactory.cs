using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace FSI.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class FSIDbContextFactory : IDesignTimeDbContextFactory<FSIDbContext>
{
    public FSIDbContext CreateDbContext(string[] args)
    {
        FSIEFCoreEntityExtensionMappings.Configure();

        var configuration = BuildConfiguration();

        var builder = new DbContextOptionsBuilder<FSIDbContext>()
            .UseOracle(configuration.GetConnectionString("DefaultConnection"));

        return new FSIDbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../FSI.WebAPI/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
