using Stargazer.Abp.Account.Application.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Stargazer.Abp.Account.Application.Services;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.MailKit;
using Volo.Abp.Modularity;

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
            context.Services.AddAutoMapperObjectMapper<StargazerAbpAccountApplicationModule>();

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<StargazerAbpAccountApplicationAutoMapperProfile>(validate: true);
            });

            context.Services.AddTransient<EmailService>();
            context.Services.BuildServiceProvider().GetRequiredService<AccountPermissionDefinitionProvider>().Define();
        }
    }
}