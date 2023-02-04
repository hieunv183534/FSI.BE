using System.Threading.Tasks;
using VNPTNET.NOM.System.Data;
using Volo.Abp.DependencyInjection;

namespace VNPTNET.NOM.System.Data;

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
