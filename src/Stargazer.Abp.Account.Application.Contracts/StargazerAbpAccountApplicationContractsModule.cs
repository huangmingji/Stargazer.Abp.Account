using Stargazer.Abp.Account.Application.Contracts.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.FluentValidation;
using Volo.Abp.Modularity;

namespace Stargazer.Abp.Account.Application.Contracts
{
    [DependsOn(
        typeof(AbpAuthorizationModule),
        typeof(AbpFluentValidationModule),
        typeof(AbpDddApplicationContractsModule)
    )]
    public class StargazerAbpAccountApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddScoped<IAccountAuthorization, AccountAuthorization>();
            AccountPermissionDefinitionProvider.Define();
        }
    }
}