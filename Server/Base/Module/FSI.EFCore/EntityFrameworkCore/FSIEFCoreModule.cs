using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.EntityFrameworkCore.Oracle;
using Volo.Abp.Modularity;

namespace FSI.EntityFrameworkCore;

[DependsOn(
    typeof(FSIDomainModule),
    typeof(AbpEntityFrameworkCoreMySQLModule)
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
            options.UseMySQL();
        });
    }
}
