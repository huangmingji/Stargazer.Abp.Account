using Stargazer.Abp.Account.Application.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Stargazer.Abp.Account.Application.Services;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.MailKit;
using Volo.Abp.Modularity;
using Stargazer.Abp.Authorization.Application.Contracts.Permissions;

namespace Stargazer.Abp.Account.Application
{
    [DependsOn(
        typeof(StargazerAbpAuthorizationApplicationContractsModule),
        typeof(StargazerAbpAccountApplicationContractsModule),
        typeof(AbpMailKitModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule)
    )]
    public class StargazerAbpAccountApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            context.Services.AddAutoMapperObjectMapper<StargazerAbpAccountApplicationModule>();

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<StargazerAbpAccountApplicationAutoMapperProfile>(validate: true);
            });

            context.Services.AddTransient<EmailService>();
            context.Services.AddTransient<AccountPermissionDefinitionProvider>();
            context.Services.GetRequiredService<AccountPermissionDefinitionProvider>().Define();
        }
    }
}