using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;
using Stargazer.Abp.Authorization.Application.Contracts.Permissions;

namespace Stargazer.Abp.Account.Application.Contracts
{
    [DependsOn(
        typeof(AbpAuthorizationModule),
        typeof(AbpDddApplicationContractsModule)
    )]
    public class StargazerAbpAuthorizationApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddScoped<IAccountAuthorization, AccountAuthorization>();
        }
    }
}