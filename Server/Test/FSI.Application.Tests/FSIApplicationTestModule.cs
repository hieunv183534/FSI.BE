using Volo.Abp.Modularity;

namespace FSI;

[DependsOn(
    typeof(FSIApplicationModule),
    typeof(FSIDomainTestModule)
    )]
public class FSIApplicationTestModule : AbpModule
{

}
