using Stargazer.Abp.Account.Application.Contracts;
using Stargazer.Abp.Account.Application.Contracts.Permissions;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace Stargazer.Abp.Account.HttpApi
{
    [DependsOn(
        typeof(StargazerAbpAccountApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule)
    )]
    public class StargazerAbpAccountHttpApiModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // context.Services.AddAuthorization(options =>
            // {
            //     foreach (var item in AccountPermissions.GetAll())
            //     {
            //         options.AddPolicy(item, policy =>
            //         {
            //             policy.RequireClaim("permisson", item);
            //         });
            //     }
            // });
            
        }
    }
}