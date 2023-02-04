using System.Threading.Tasks;

namespace FSI.Data;

public interface IFSIDbSchemaMigrator
{
    Task MigrateAsync();
}
