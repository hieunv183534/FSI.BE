using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace FSI
{
    [DependsOn(
    typeof(FSIDomainModule),
    typeof(FSIApplicationContractsModule),
    typeof(AbpAutoMapperModule)
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
