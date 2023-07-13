using FSI.GrpcClient.RecommendationSystem;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.AutoMapper;
using Volo.Abp.EventBus.RabbitMq;
using Volo.Abp.Modularity;

namespace FSI
{
    [DependsOn(
    typeof(FSIDomainModule),
    typeof(FSIApplicationContractsModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpEventBusRabbitMqModule)
    )]
    public class FSIApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<FSIApplicationModule>();
            });

            context.Services.AddScoped<IRecommendationSystem, RecommendationSystem>();
        }

    }
}
