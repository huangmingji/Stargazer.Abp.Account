using System;
using System.Reflection;
using Stargazer.Abp.Account.Domain.Shared.Localization.Resources;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Volo.Abp.VirtualFileSystem;

namespace Stargazer.Abp.Account.Domain.Shared
{
    [DependsOn(
        typeof(AbpLocalizationModule),
        typeof(AbpValidationModule))]
    public class StargazerAbpAccountDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<StargazerAbpAccountDomainSharedModule>(baseNamespace: "Stargazer.Abp.Account.Domain.Shared");
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<AccountResource>("zh-Hans")
                    .AddVirtualJson("/Localization/Resources/Account");
            });
        }
    }
}
