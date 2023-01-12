using System.Threading.Tasks;

namespace FSI.Data;

public interface IDbSchemaMigrator
{
    Task MigrateAsync();
}
