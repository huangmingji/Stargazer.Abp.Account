using Stargazer.Abp.Account.Domain.Shared.Localization.Resources;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using static Stargazer.Abp.Account.Application.Contracts.Authorization.AccountPermissions;

namespace Stargazer.Abp.Account.Application;

public class AccountPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var accountGroup = context.AddGroup("account", L("user center"));
        var userPermissionDefinition = accountGroup.AddPermission(User.Manage, L("user management"));
        userPermissionDefinition.AddChild(User.Create, L("create user"));
        userPermissionDefinition.AddChild(User.Update, L("update user"));
        userPermissionDefinition.AddChild(User.Delete, L("delete user"));

        var rolePermissionDefinition = accountGroup.AddPermission(Role.Manage, L("role management"));
        rolePermissionDefinition.AddChild(Role.Create, L("create role"));
        rolePermissionDefinition.AddChild(Role.Update, L("update role"));
        rolePermissionDefinition.AddChild(Role.Delete, L("delete role"));

        var permissionPermissionDefinition = accountGroup.AddPermission(Permission.Manage, L("permission management"));
        permissionPermissionDefinition.AddChild(Permission.Create, L("create permission"));
        permissionPermissionDefinition.AddChild(Permission.Update, L("update permission"));
        permissionPermissionDefinition.AddChild(Permission.Delete, L("delete permission"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AccountResource>(name);
    }
}