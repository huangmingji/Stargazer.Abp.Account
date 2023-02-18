using Stargazer.Abp.Account.Application.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.MailKit;
using Volo.Abp.Modularity;

namespace Stargazer.Abp.Account.Application
{
    [DependsOn(
        typeof(LemonAccountApplicationContractsModule),
        typeof(AbpMailKitModule),
        typeof(AbpDddApplicationModule),
        typeof(AbpAutoMapperModule)
    )]
    public class LemonAccountApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();

            context.Services.AddAutoMapperObjectMapper<LemonAccountApplicationModule>();

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<LemonAccountApplicationAutoMapperProfile>(validate: true);
            });
        }
    }
}