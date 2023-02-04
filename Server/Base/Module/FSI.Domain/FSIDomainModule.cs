using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Volo.Abp.Modularity;

namespace VNPTNET.NOM.System
{
    [DependsOn(
       typeof(FSIDomainSharedModule)
   )]
    public class FSIDomainModule : AbpModule
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
        }
    }
}
