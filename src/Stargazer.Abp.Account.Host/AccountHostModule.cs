using System;
using System.Linq;
using Stargazer.Abp.Account.Application;
using Stargazer.Abp.Account.EntityFrameworkCore.DbMigrations;
using Stargazer.Abp.Account.HttpApi;
using Lemon.Common.Extend;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace Stargazer.Abp.Account.Host
{
    [DependsOn(
    typeof(StargazerAbpAccountEntityFrameworkCoreDbMigrationsModule),
    typeof(StargazerAbpAccountApplicationModule),
    typeof(StargazerAbpAccountHttpApiModule),
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpAutofacModule),
    typeof(AbpAspNetCoreSerilogModule)
    )]
    public class StargazerAbpAccountHostModule : AbpModule
    {
        private const string DefaultCorsPolicyName = "Default";
        private static void ConfigureSwaggerServices(ServiceConfigurationContext context)
        {
            context.Services.AddSwaggerGen(
                options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Account Service API", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);
                });
        }

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
            
            ConfigureSwaggerServices(context);
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
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Account Service API");
                });
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

