using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VNPTNET.NOM.System.Data;
using Volo.Abp.DependencyInjection;

namespace VNPTNET.NOM.System.EntityFrameworkCore;

public class EntityFrameworkCoreFSIDbSchemaMigrator
    : IFSIDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreFSIDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the NOMDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<FSIDbContext>()
            .Database
            .MigrateAsync();
    }
}
