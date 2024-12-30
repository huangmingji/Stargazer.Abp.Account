using Stargazer.Abp.Account.Application;
using Stargazer.Abp.Account.HttpApi;
using Lemon.Common.Extend;
using Microsoft.AspNetCore.Cors;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Stargazer.Abp.Account.EntityFrameworkCore;

namespace Stargazer.Abp.Account.Host
{
    [DependsOn(
    typeof(StargazerAbpAccountEntityFrameworkCoreModule),
    typeof(StargazerAbpAccountApplicationModule),
    typeof(StargazerAbpAccountHttpApiModule),
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpAutofacModule),
    typeof(AbpAspNetCoreSerilogModule)
    )]
    public class StargazerAbpAccountHostModule : AbpModule
    {
        private const string DefaultCorsPolicyName = "Default";
        private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
        {
            context.Services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorsPolicyName, builder =>
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

        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            var hostingEnvironment = context.Services.GetHostingEnvironment();

            context.Services.AddMvcCore().AddNewtonsoftJson(
                op =>
                {
                    op.SerializerSettings.ContractResolver =
                        new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
                    op.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    op.SerializerSettings.Converters.Add(new Ext.DateTimeJsonConverter());
                    op.SerializerSettings.Converters.Add(new Ext.LongJsonConverter());
                });
            
            // Configure<AbpAspNetCoreMvcOptions>(options =>
            // {
            //     options
            //         .ConventionalControllers
            //         .Create(typeof(StargazerAbpAccountApplicationModule).Assembly, opts =>
            //         {
            //             opts.RootPath = "account";
            //         });
            // });
            
            ConfigureCors(context, configuration);

            // #region 由于不打算使用abp vnext的ids4，所以需要在这里注入ICurrentUser和ICurrentTenant
            
            // context.Services.AddHttpContextAccessor();
            // context.Services.AddTransient<ICurrentUser, CurrentUser>();
            // context.Services.AddTransient<ICurrentTenant, CurrentTenant>();

            // #endregion
        }

        public override void OnApplicationInitialization(
            ApplicationInitializationContext context)
        {
            var app = context.GetApplicationBuilder();
            var env = context.GetEnvironment();

            if (env.IsProduction())
            {
                app.UseExceptionHandler("/Error");
            }
            else
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(DefaultCorsPolicyName);

            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseConfiguredEndpoints();

        }
    }
}

