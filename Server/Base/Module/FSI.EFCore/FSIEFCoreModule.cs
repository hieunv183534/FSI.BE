using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace VNPTNET.NOM.System;

[DependsOn(
    typeof(FSIDomainModule)
    )]
public class FSIEFCoreModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
    }
}
