using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.AuditLogging;
using Volo.Abp.AutoMapper;
using Volo.Abp.Emailing;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Modularity;
using Volo.Abp.SettingManagement;

namespace FSI
{
    [DependsOn(
       typeof(DomainSharedModule),
       typeof(AbpAuditLoggingDomainModule),
       typeof(AbpFeatureManagementDomainModule),
       typeof(AbpSettingManagementDomainModule),
       typeof(AbpEmailingModule),
       typeof(AbpAutoMapperModule)
   )]
    public class DomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {

            //Configure<AbpMultiTenancyOptions>(options =>
            //{
            //    options.IsEnabled = MultiTenancyConsts.IsEnabled;
            //});

            //#if DEBUG
            //        context.Services.Replace(ServiceDescriptor.Singleton<IEmailSender, NullEmailSender>());
            //#endif
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<DomainModule>();
            });
        }
    }
}
