using Stargazer.Abp.Account.Domain.Shared;
using Stargazer.Abp.Account.Domain.Shared.MultiTenancy;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;

namespace Stargazer.Abp.Account.Domain
{
    [DependsOn(
        typeof(LemonAccountDomainSharedModule),
        typeof(AbpDddDomainModule))]
    public class LemonAccountDomainModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpMultiTenancyOptions>(options =>
            {
                options.IsEnabled = MultiTenancyConsts.IsEnabled;
            });
        }
    }
}