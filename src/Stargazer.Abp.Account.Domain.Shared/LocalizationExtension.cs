using Stargazer.Abp.Account.Domain.Shared.Localization.Resources;
using Volo.Abp.Localization;

namespace Stargazer.Abp.Account.Domain.Shared;

public class LocalizationExtension
{

    public static LocalizableString L(string name)
    {
        return LocalizableString.Create<AccountResource>(name);
    }

}