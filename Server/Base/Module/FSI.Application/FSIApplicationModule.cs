using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace VNPTNET.NOM.System
{
    [DependsOn(
    typeof(FSIDomainModule),
    typeof(FSIApplicationContractsModule)
    )]
    public class FSIApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<FSIApplicationModule>();
            });
        }
    }
}
