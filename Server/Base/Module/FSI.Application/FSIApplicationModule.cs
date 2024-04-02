using FSI.Application.Agora;
using FSI.Application.Mailling;
using FSI.GrpcClient.RecommendationSystem;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.AutoMapper;
using Volo.Abp.BackgroundJobs.RabbitMQ;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.Azure;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.EventBus.RabbitMq;
using Volo.Abp.Modularity;

namespace FSI
{
    [DependsOn(
    typeof(FSIDomainModule),
    typeof(FSIApplicationContractsModule),
    typeof(AbpAutoMapperModule),
    //typeof(AbpEventBusRabbitMqModule),
    //typeof(AbpBackgroundJobsRabbitMqModule),
    typeof(AbpCachingStackExchangeRedisModule),
    typeof(AbpBlobStoringAzureModule)
    )]
    public class FSIApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddMaps<FSIApplicationModule>();
            });

            Configure<AbpBlobStoringOptions>(options =>
            {
                options.Containers.ConfigureDefault(container =>
                {
                    container.UseAzure(azure =>
                    {
                        azure.ConnectionString = configuration["AzureBlobContainer:ConnectionString"];
                        azure.ContainerName = configuration["AzureBlobContainer:ContainerName"];
                        azure.CreateContainerIfNotExists = true;
                    });
                });
            });

            context.Services.AddScoped<IRecommendationSystem, RecommendationSystem>();
            context.Services.AddScoped<ISendMailService, SendMailService>();
            context.Services.AddSingleton<MeetHubService>();

            var mailsettings = configuration.GetSection("MailSettings");
            context.Services.Configure<MailSettings>(mailsettings);
        }

    }
}
