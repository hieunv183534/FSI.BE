using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace FSI.Data;

/* This is used if database provider does't define
 * INOMDbSchemaMigrator implementation.
 */
public class NullDbSchemaMigrator : IDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
