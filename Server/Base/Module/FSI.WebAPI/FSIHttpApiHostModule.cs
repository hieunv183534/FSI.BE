using Microsoft.AspNetCore.Cors;
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
using Volo.Abp.AspNetCore.ExceptionHandling;
using Newtonsoft.Json.Linq;
using Volo.Abp.Json.SystemTextJson;
using FSI.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Volo.Abp.AspNetCore.SignalR;
using FSI.Application.Hubs;
using Microsoft.AspNetCore.Http.Features;
using System.Text.Json.Serialization;
using FSI.Application.Agora;

namespace FSI
{
    [DependsOn(
       typeof(FSIHttpApiModule),
       typeof(AbpAutofacModule),
       typeof(FSIApplicationModule),
       typeof(FSIEFCoreModule),
       typeof(AbpAspNetCoreAuthenticationJwtBearerModule),
       typeof(AbpAspNetCoreSerilogModule),
       typeof(AbpSwashbuckleModule),
       typeof(AbpAspNetCoreSignalRModule))]
    public class FSIHttpApiHostModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            ConfigureBundles();
            ConfigureUrls(configuration);
            ConfigureConventionalControllers();
            ConfigureLocalization();
            ConfigureVirtualFileSystem(context);

            context.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("this-is-my-super-key")),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

                x.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            (path.StartsWithSegments("/test") ||
                            path.StartsWithSegments("/chat") ||
                            path.StartsWithSegments("/meet")))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            Configure<FormOptions>(options =>
            {
                options.MemoryBufferThreshold = Int32.MaxValue;
            });

            Configure<AbpSignalROptions>(options =>
            {
                options.Hubs.Add(
                    new HubConfig(
                        typeof(TestHub),
                        "/test",
                        hubOptions =>
                        {
                            hubOptions.LongPolling.PollTimeout = TimeSpan.FromSeconds(30);
                        }
                    )
                );

                options.Hubs.Add(
                    new HubConfig(
                        typeof(ChatHub),
                        "/chat",
                        hubOptions =>
                        {
                            hubOptions.LongPolling.PollTimeout = TimeSpan.FromSeconds(30);
                        }
                    )
                );

                options.Hubs.Add(
                    new HubConfig(
                        typeof(MeetHub),
                        "/meet",
                        hubOptions =>
                        {
                            hubOptions.LongPolling.PollTimeout = TimeSpan.FromSeconds(30);
                        }
                    )
                );
            });

            ConfigureCors(context, configuration);
            ConfigureSwaggerServices(context, configuration);
            context.Services.AddSingleton<IHttpExceptionStatusCodeFinder, FSIHttpExceptionStatusCodeFinder>();
            Configure<AbpSystemTextJsonSerializerOptions>(opt =>
            {
                opt.UnsupportedTypes.Add(typeof(JArray));
                opt.UnsupportedTypes.Add(typeof(JObject));
            });
        }

        private void ConfigureBundles()
        {
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
                    options.FileSets.ReplaceEmbeddedByPhysical<FSIDomainSharedModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}FSI.Domain.Shared"));
                    options.FileSets.ReplaceEmbeddedByPhysical<FSIDomainModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}FSI.Domain"));
                    options.FileSets.ReplaceEmbeddedByPhysical<FSIApplicationContractsModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}FSI.Application.Contracts"));
                    options.FileSets.ReplaceEmbeddedByPhysical<FSIApplicationModule>(
                        Path.Combine(hostingEnvironment.ContentRootPath,
                            $"..{Path.DirectorySeparatorChar}FSI.Application"));
                });
            }
        }

        private void ConfigureConventionalControllers()
        {
            Configure<AbpAspNetCoreMvcOptions>(options =>
            {
                options.ConventionalControllers.Create(typeof(FSIApplicationModule).Assembly, opts =>
                {
                    opts.RootPath = "fsi";
                });
            });
        }

        private static void ConfigureSwaggerServices(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddAbpSwaggerGen().AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "FSI API", Version = "v1" });
                options.DocInclusionPredicate((docName, description) => true);
                options.CustomSchemaIds(type => type.FullName);
                var securityScheme = new OpenApiSecurityScheme()
                {
                    Name = "JWT Authentication",
                    Description = "Enter a valid jwt bearer token",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference()
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }

                };
                options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {securityScheme , new string[] {}}
                });
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
                        .WithAbpExposedHeaders()
                        .WithExposedHeaders("Content-Disposition")
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .SetIsOriginAllowed((host) => true)
                        .AllowCredentials();
                });
            });
        }
        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAbpRequestLocalization();

            app.UseCorrelationId();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();
            app.UseUnitOfWork();

            app.UseSwagger();
            app.UseAbpSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "FSI API");
            });
            app.UseAuditing();
            app.UseAbpSerilogEnrichers();
            app.UseConfiguredEndpoints();
        }

        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
        }
    }
}
