using System;
using Volo.Abp.Localization;
using Stargazer.Abp.Account.Domain.Shared.Localization.Resources;
using Volo.Abp.Authorization.Permissions;
using static Stargazer.Abp.Account.Application.Contracts.Authorization.AccountPermissions;

namespace Stargazer.Abp.Account.Application.Contracts.Authorization
{
	public class AccountPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public AccountPermissionDefinitionProvider()
		{
		}

        public override void Define(IPermissionDefinitionContext context)
        {
            var userGroup = context.AddGroup(User.Manage, L("用户管理"));
            userGroup.AddPermission(User.Create, L("account:user:create"));
            userGroup.AddPermission(User.Update, L("account:user:update"));
            userGroup.AddPermission(User.Delete, L("account:user:delete"));

            var roleGroup = context.AddGroup(Role.Manage, L("account:role:manage"));
            roleGroup.AddPermission(Role.Create, L("account:role:create"));
            roleGroup.AddPermission(Role.Update, L("account:role:update"));
            roleGroup.AddPermission(Role.Delete, L("account:role:delete"));

            var permissionGroup = context.AddGroup(Permission.Manage, L("account:permission:manage"));
            permissionGroup.AddPermission(Permission.Create, L("account:permission:create"));
            permissionGroup.AddPermission(Permission.Update, L("account:permission:update"));
            permissionGroup.AddPermission(Permission.Delete, L("account:permission:delete"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<AccountResource>(name);
        }
    }
}

