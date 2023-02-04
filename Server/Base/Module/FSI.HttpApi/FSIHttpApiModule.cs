using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using VNPTNET.NOM.System.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace VNPTNET.NOM.System
{
    [DependsOn(
       typeof(FSIApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule)
       )]
    public class FSIHttpApiModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            ConfigureLocalization();
        }

        private void ConfigureLocalization()
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Get<FSIResource>()
                    .AddBaseTypes(
                        typeof(AbpUiResource)
                    );
            });
        }
    }
}
