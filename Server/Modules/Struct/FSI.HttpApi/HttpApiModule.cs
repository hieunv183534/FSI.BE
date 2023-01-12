using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using FSI.Localization;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;

namespace FSI
{
    [DependsOn(
       typeof(ApplicationContractsModule),
       typeof(AbpFeatureManagementHttpApiModule),
       typeof(AbpSettingManagementHttpApiModule)
       )]
    public class HttpApiModule : AbpModule
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
                    .Get<Resource>()
                    .AddBaseTypes(
                        typeof(AbpUiResource)
                    );
                options.Resources
                    .Get<ExRecource>();
            });
        }
    }
}