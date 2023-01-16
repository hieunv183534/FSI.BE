using Microsoft.AspNetCore.Cors;
using FSI.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Volo.Abp;
using Volo.Abp.AspNetCore.Authentication.JwtBearer;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;
using Volo.Abp.UI.Navigation.Urls;
using Volo.Abp.VirtualFileSystem;
using Steeltoe.Discovery.Client;
using Volo.Abp.AspNetCore.ExceptionHandling;

namespace FSI
{
    [DependsOn(
       typeof(HttpApiModule),
       typeof(AbpAutofacModule),
       typeof(ApplicationModule),
       typeof(EntityFrameworkCoreModule),
       typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
       typeof(AbpAspNetCoreSerilogModule),
       typeof(AbpSwashbuckleModule))]
    public class HttpApiHostModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var test = configuration.GetSection("spring").GetSection("application").GetSection("name").Value;
            var hostingEnvironment = context.Services.GetHostingEnvironment();

            ConfigureBundles();
            ConfigureUrls(configuration);
            ConfigureConventionalControllers();
            //ConfigureAuthentication(context, configuration);
            ConfigureLocalization();
            ConfigureVirtualFileSystem(context);
            ConfigureCors(context, configuration);
            //ConfigureEureka(context, configuration); 
            ConfigureSwaggerServices(context, configuration);

            context.Services.AddSingleton<IHttpExceptionStatusCodeFinder, HttpExceptionStatusCodeFinder>();
            //ConfigureHangfire(context, configuration);
            //Configure<AbpRabbitMqOptions>(options =>
            //{
            //    options.Connections.Default.UserName = "admin";
            //    options.Connections.Default.Password = "1q2w3E*";
            //    options.Connections.Default.HostName = "localhost";
            //    options.Connections.Default.Port = 44358;
            //});
            //Configure<AbpBackgroundJobQuartzOptions>(options =>
            //{
            //    options.RetryCount = 1;
            //    options.RetryIntervalMillisecond = 1000;
            //});
        }

        private void ConfigureEureka(ServiceConfigurationContext context,IConfiguration configuration)
        {
            context.Services.AddDiscoveryClient(configuration);
        }

        private void ConfigureBundles()
        {
            //Configure<AbpBundlingOptions>(options =>
            //{
            //    options.StyleBundles.Configure(
            //        BasicThemeBundles.Styles.Global,
            //        bundle => { bundle.AddFiles("/global-styles.css"); }
            //    );
            //});
        }

        private void ConfigureUrls(IConfiguration configuration)
        {
            Configure<AppUrlOptions>(options =>
            {
                options.Applications["MVC"].RootUrl = configuration["App:SelfUrl"];
                options.RedirectAllowedUrls.AddRange(configuration["App:RedirectAllowedUrls"].Split(','));

                options.Applications["Angular"].RootUrl = configuration["App:ClientUrl"];
                //options.Applications["Angular"].Urls[AccountUrlNames.PasswordReset] = "account/reset-password";
            }); 
        }

        private void ConfigureVirtualFileSystem(ServiceConfigurationContext context)
        {
            var hostingEnvironment = context.Services.GetHostingEnvironment();

            if (hostingEnvironment.IsDevelopment())
            {
                Configure<AbpVirtualFileSystemOptions>(options =>
                {
                    options.FileSets.ReplaceEmbeddedByPhysical<DomainSharedModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}FSI.Domain.Shared"));
                    options.FileSets.ReplaceEmbeddedByPhysical<DomainModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}FSI.Domain"));
                    options.FileSets.ReplaceEmbeddedByPhysical<ApplicationContractsModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}FSI.Application.Contracts"));
                    options.FileSets.ReplaceEmbeddedByPhysical<ApplicationModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}FSI.Application"));
                });
            }
        }

        private void ConfigureConventionalControllers()
        {
            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(typeof(ApplicationModule).Assembly, opts =>
                {
                    opts.RootPath = "nom/struct";
                });
            });
        }

        private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
        {
            //context.Services.AddAuthentication()
            //    .AddJwtBearer(options =>
            //    {
            //        options.Authority = configuration["AuthServer:Authority"];
            //        options.RequireHttpsMetadata = Convert.ToBoolean(configuration["AuthServer:RequireHttpsMetadata"]);
            //        options.Audience = "NOM";
            //        options.BackchannelHttpHandler = new HttpClientHandler
            //        {
            //            ServerCertificateCustomValidationCallback =
            //                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            //        };
            //    });
        }

        private static void ConfigureSwaggerServices(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddAbpSwaggerGen().AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "NOM Struct API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
                //options.AddSecurityDefinition("Bearer",
                //new OpenApiSecurityScheme
                //{
                //    In = ParameterLocation.Header,
                //    Description = "Please enter into field the word 'Bearer' following by space and JWT",
                //    Name = "Authorization",
                //    Type = SecuritySchemeType.ApiKey
                //});
                //options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                //    {
                //        new OpenApiSecurityScheme
                //        {
                //            Reference = new OpenApiReference
                //            {
                //                Type = ReferenceType.SecurityScheme,
                //                Id = "Bearer"
                //            }
                //        },
                //        Array.Empty<string>()
                //    } });
            });
        }

        private void ConfigureLocalization()
        {
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Languages.Add(new LanguageInfo("en", "en", "English"));
                options.Languages.Add(new LanguageInfo("vi", "vi", "Việt Nam"));
            });
        }

        private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                        .WithOrigins(
                            configuration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        )
                        .WithAbpExposedHeaders()
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });
        }
        //private void ConfigureHangfire(ServiceConfigurationContext context, IConfiguration configuration)
        //{
        //    context.Services.AddHangfire(config =>
        //    {
        //        config.UseSqlServerStorage(configuration.GetConnectionString("DefaultSQL"));
        //    });
        //}
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAbpRequestLocalization();

            //if (!env.IsDevelopment())
            //{
            //    app.UseErrorPage();
            //}

            app.UseCorrelationId();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors();
            //app.UseAuthentication();
            //app.UseJwtTokenMiddleware();


            app.UseUnitOfWork();
            //app.UseIdentityServer();
            //app.UseAuthorization();

            app.UseSwagger();
            app.UseAbpSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "NOM Struct API");

                //var configuration = context.ServiceProvider.GetRequiredService<IConfiguration>();
                //c.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
                //c.OAuthClientSecret(configuration["AuthServer:SwaggerClientSecret"]);
                //c.OAuthScopes("NOM");
            });

            app.UseAuditing();
            app.UseAbpSerilogEnrichers();
            app.UseConfiguredEndpoints();
            //app.UseHangfireDashboard();
        }

        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
        }
    }
}
