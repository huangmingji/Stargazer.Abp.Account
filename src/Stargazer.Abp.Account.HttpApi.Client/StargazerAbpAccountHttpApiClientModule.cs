using System;
using Stargazer.Abp.Account.Application.Contracts;
using Stargazer.Abp.Account.Domain.Shared;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace Stargazer.Abp.Account.HttpApi.Client
{
    [DependsOn(
        typeof(LemonAccountApplicationContractsModule),
        typeof(AbpHttpClientModule))]
    public class LemonAccountHttpApiClientModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            // var configuration = context.Services.GetConfiguration();
            // context.Services.AddHttpClient();
            // context.Services.AddHttpClient("account", config =>
            // {
            //     config.BaseAddress= new Uri(configuration.GetSection("RemoteServices:Account:BaseUrl").Value);
            //     config.DefaultRequestHeaders.Add("Accept", "application/json");
            // });
            // context.Services.AddSingleton<IAccountService, AccountService>();
            context.Services.AddHttpClientProxies(
                typeof(LemonAccountApplicationContractsModule).Assembly,
                "Account"
            );
        }
    }
}
