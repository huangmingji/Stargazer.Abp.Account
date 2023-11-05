using System;
using Volo.Abp.Localization;
using Stargazer.Abp.Account.Domain.Shared.Localization.Resources;
using Volo.Abp.Authorization.Permissions;
using static Stargazer.Abp.Account.Application.Contracts.Authorization.AccountPermissions;

namespace Stargazer.Abp.Account.Application.Contracts.Authorization
{
	public class AccountPermissionDefinitionProvider
    {
        public static void Define()
        {
            PermissionDefinitionProvider.AddGroup(User.Manage, L("User management"))
                .AddPermission(User.Create, L("Create user"))
                .AddPermission(User.Update, L("Update user"))
                .AddPermission(User.Delete, L("Delete user"));
            PermissionDefinitionProvider.AddGroup(Role.Manage, L("Role management"))
                .AddPermission(Role.Create, L("Create role"))
                .AddPermission(Role.Update, L("Update role"))
                .AddPermission(Role.Delete, L("Delete role"));
            PermissionDefinitionProvider.AddGroup(Permission.Manage, L("Permission management"))
                .AddPermission(Permission.Create, L("Create permission"))
                .AddPermission(Permission.Update, L("Update permission"))
                .AddPermission(Permission.Delete, L("Delete permission"));

        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<AccountResource>(name);
        }
    }
}

