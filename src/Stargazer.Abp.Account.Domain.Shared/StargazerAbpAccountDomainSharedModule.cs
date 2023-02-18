using System;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;

namespace Stargazer.Abp.Account.Domain.Shared
{
    [DependsOn(
        typeof(AbpValidationModule))]
    public class LemonAccountDomainSharedModule : AbpModule
    {
        public LemonAccountDomainSharedModule()
        {
        }
    }
}
