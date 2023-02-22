using Stargazer.Abp.Account.Application.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.MailKit;
using Volo.Abp.Modularity;

namespace Stargazer.Abp.Account.Application
{
    [DependsOn(
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
        }
    }
}