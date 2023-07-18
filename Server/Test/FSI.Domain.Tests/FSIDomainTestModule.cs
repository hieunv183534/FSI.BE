using FSI.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace FSI;

[DependsOn(
    typeof(FSIEntityFrameworkCoreTestModule)
    )]
public class FSIDomainTestModule : AbpModule
{

}
