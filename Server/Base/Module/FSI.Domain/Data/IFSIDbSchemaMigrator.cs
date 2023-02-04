using System.Threading.Tasks;

namespace VNPTNET.NOM.System.Data;

public interface IFSIDbSchemaMigrator
{
    Task MigrateAsync();
}
