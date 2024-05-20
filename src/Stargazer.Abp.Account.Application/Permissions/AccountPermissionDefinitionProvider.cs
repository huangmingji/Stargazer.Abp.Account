using Stargazer.Abp.Account.Domain.Shared;
using Volo.Abp.Authorization.Permissions;
using static Stargazer.Abp.Account.Application.Contracts.Authorization.AccountPermissions;

namespace Stargazer.Abp.Account.Application;

public class AccountPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var accountGroup = context.AddGroup("account", LocalizationExtension.L("UserCenter"));
        var userPermissionDefinition = accountGroup.AddPermission(User.Manage, LocalizationExtension.L("UserManagement"));
        userPermissionDefinition.AddChild(User.Create, LocalizationExtension.L("CreateUser"));
        userPermissionDefinition.AddChild(User.Update, LocalizationExtension.L("UpdateUser"));
        userPermissionDefinition.AddChild(User.Delete, LocalizationExtension.L("DeleteUser"));

        var rolePermissionDefinition = accountGroup.AddPermission(Role.Manage, LocalizationExtension.L("RoleManagement"));
        rolePermissionDefinition.AddChild(Role.Create, LocalizationExtension.L("CreateRole"));
        rolePermissionDefinition.AddChild(Role.Update, LocalizationExtension.L("UpdateRole"));
        rolePermissionDefinition.AddChild(Role.Delete, LocalizationExtension.L("DeleteRole"));

        var permissionPermissionDefinition = accountGroup.AddPermission(Permission.Manage, LocalizationExtension.L("PermissionManagement"));
        permissionPermissionDefinition.AddChild(Permission.Create, LocalizationExtension.L("CreatePermission"));
        permissionPermissionDefinition.AddChild(Permission.Update, LocalizationExtension.L("UpdatePermission"));
        permissionPermissionDefinition.AddChild(Permission.Delete, LocalizationExtension.L("DeletePermission"));
    }
}