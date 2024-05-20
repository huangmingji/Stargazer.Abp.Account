using Stargazer.Abp.Account.Domain.Shared.Localization.Resources;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using static Stargazer.Abp.Account.Application.Contracts.Authorization.AccountPermissions;

namespace Stargazer.Abp.Account.Application;

public class AccountPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var accountGroup = context.AddGroup("account", L("UserCenter"));
        var userPermissionDefinition = accountGroup.AddPermission(User.Manage, L("UserManagement"));
        userPermissionDefinition.AddChild(User.Create, L("CreateUser"));
        userPermissionDefinition.AddChild(User.Update, L("UpdateUser"));
        userPermissionDefinition.AddChild(User.Delete, L("DeleteUser"));

        var rolePermissionDefinition = accountGroup.AddPermission(Role.Manage, L("RoleManagement"));
        rolePermissionDefinition.AddChild(Role.Create, L("CreateRole"));
        rolePermissionDefinition.AddChild(Role.Update, L("UpdateRole"));
        rolePermissionDefinition.AddChild(Role.Delete, L("DeleteRole"));

        var permissionPermissionDefinition = accountGroup.AddPermission(Permission.Manage, L("PermissionManagement"));
        permissionPermissionDefinition.AddChild(Permission.Create, L("CreatePermission"));
        permissionPermissionDefinition.AddChild(Permission.Update, L("UpdatePermission"));
        permissionPermissionDefinition.AddChild(Permission.Delete, L("DeletePermission"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<AccountResource>(name);
    }
}