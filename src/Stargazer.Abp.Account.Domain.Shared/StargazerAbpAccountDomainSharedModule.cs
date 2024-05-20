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
                options.FileSets.AddEmbedded<StargazerAbpAccountDomainSharedModule>();
            });
            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<AccountResource>("zh-Hans")
                    .AddVirtualJson("/Localization/Resources/Account");

                options.Languages.Add(new LanguageInfo("en", "en", "English"));
                options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
                foreach (var key in options.Resources.Keys)
                {
                    options.Resources[key].DefaultCultureName = "zh-Hans";
                }
            });
        }
    }
}
