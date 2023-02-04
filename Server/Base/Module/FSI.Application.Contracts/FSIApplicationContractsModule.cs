using Volo.Abp.Modularity;
using Volo.Abp.ObjectExtending;

namespace VNPTNET.NOM.System
{
    [DependsOn(
    typeof(FSIDomainSharedModule),
    typeof(AbpObjectExtendingModule)
)]
    public class FSIApplicationContractsModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            FSIDtoExtensions.Configure();
        }
    }
}
