using Microsoft.Extensions.Localization;
using Stargazer.Abp.Account.Domain.Shared;
using Stargazer.Abp.Account.Domain.Shared.Localization.Resources;
using Stargazer.Abp.Authorization.Application.Contracts.Permissions;
using static Stargazer.Abp.Account.Application.Contracts.Authorization.AccountPermissions;

namespace Stargazer.Abp.Account.Application;

public class AccountPermissionDefinitionProvider : PermissionDefinitionProvider
{
    private readonly IStringLocalizer<AccountResource> _localizer;

    public AccountPermissionDefinitionProvider(IStringLocalizer<AccountResource> localizer)
    {
        _localizer = localizer;
    }

    public override void Define()
    {
        var accountGroup = PermissionGroupDefinition.CreateGroup("account", _localizer["UserCenter"]);
        var userPermissionDefinition = accountGroup.AddPermission(User.Manage, _localizer["UserManagement"]);
        userPermissionDefinition.AddChild(User.Create, _localizer["CreateUser"]);
        userPermissionDefinition.AddChild(User.Update, _localizer["UpdateUser"]);
        userPermissionDefinition.AddChild(User.Delete, _localizer["DeleteUser"]);

        var rolePermissionDefinition = accountGroup.AddPermission(Role.Manage, _localizer["RoleManagement"]);
        rolePermissionDefinition.AddChild(Role.Create, _localizer["CreateRole"]);
        rolePermissionDefinition.AddChild(Role.Update, _localizer["UpdateRole"]);
        rolePermissionDefinition.AddChild(Role.Delete, _localizer["DeleteRole"]);

        var permissionPermissionDefinition = accountGroup.AddPermission(Permission.Manage, _localizer["PermissionManagement"]);
        permissionPermissionDefinition.AddChild(Permission.Create, _localizer["CreatePermission"]);
        permissionPermissionDefinition.AddChild(Permission.Update, _localizer["UpdatePermission"]);
        permissionPermissionDefinition.AddChild(Permission.Delete, _localizer["DeletePermission"]);
    }
}