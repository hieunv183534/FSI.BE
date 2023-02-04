using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Oracle;
using Volo.Abp.Modularity;

namespace VNPTNET.NOM.System.EntityFrameworkCore;

[DependsOn(
    typeof(FSIDomainModule),
    typeof(AbpEntityFrameworkCoreOracleModule)
    )]
public class FSIEFCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        FSIEFCoreEntityExtensionMappings.Configure();
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<FSIDbContext>(options =>
        {
                /* Remove "includeAllEntities: true" to create
                 * default repositories only for aggregate roots */
            options.AddDefaultRepositories(includeAllEntities: true);
        });

        Configure<AbpDbContextOptions>(options =>
        {
                /* The main point to change your DBMS.
                 * See also NOMMigrationsDbContextFactory for EF Core tooling. */
            //options.UseSqlServer();
            options.UseOracle();// b => b.UseOracleSQLCompatibility("19")
        });
    }
}
