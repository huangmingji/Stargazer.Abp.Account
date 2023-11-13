using Volo.Abp.Localization;
using Stargazer.Abp.Account.Domain.Shared.Localization.Resources;
using static Stargazer.Abp.Account.Application.Contracts.Authorization.AccountPermissions;

namespace Stargazer.Abp.Account.Application.Contracts.Authorization
{
	public class AccountPermissionDefinitionProvider
    {
        public static void Define()
        {
            PermissionDefinitionProvider.AddGroup(User.Manage, L("User management"))
                .AddPermission(User.Create, L("Create"))
                .AddPermission(User.Update, L("Update"))
                .AddPermission(User.Delete, L("Delete"));
            PermissionDefinitionProvider.AddGroup(Role.Manage, L("Role management"))
                .AddPermission(Role.Create, L("Create"))
                .AddPermission(Role.Update, L("Update"))
                .AddPermission(Role.Delete, L("Delete"));
            PermissionDefinitionProvider.AddGroup(Permission.Manage, L("Permission management"))
                .AddPermission(Permission.Create, L("Create"))
                .AddPermission(Permission.Update, L("Update"))
                .AddPermission(Permission.Delete, L("Delete"));

        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<AccountResource>(name);
        }
    }
}

