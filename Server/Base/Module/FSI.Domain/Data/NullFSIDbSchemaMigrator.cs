using System.Threading.Tasks;
using FSI.Data;
using Volo.Abp.DependencyInjection;

namespace FSI.Data;

/* This is used if database provider does't define
 * INOMDbSchemaMigrator implementation.
 */
public class NullFSIDbSchemaMigrator : IFSIDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
