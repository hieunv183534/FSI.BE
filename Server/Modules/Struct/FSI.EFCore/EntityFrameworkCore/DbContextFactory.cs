using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace FSI.EntityFrameworkCore;

/* This class is needed for EF Core console commands
 * (like Add-Migration and Update-Database commands) */
public class DbContextFactory : IDesignTimeDbContextFactory<DbContext>
{
    public DbContext CreateDbContext(string[] args)
    {
        EfCoreEntityExtensionMappings.Configure();

        var configuration = BuildConfiguration();

        //var oracleVersion = configuration.GetSection("OracleSettings").GetSection("OracleVersion").Value;
        var builder = new DbContextOptionsBuilder<DbContext>()
            .UseOracle(configuration.GetConnectionString("DefaultConnection"));//, b => b.UseOracleSQLCompatibility("19"));
            //.UseSqlServer(configuration.GetConnectionString("Default"));

        return new DbContext(builder.Options);
    }

    private static IConfigurationRoot BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
            //.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../VNPT.NOM.Struct.DbMigrator/"))
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../FSI.WebAPI/"))
            .AddJsonFile("appsettings.json", optional: false);

        return builder.Build();
    }
}
