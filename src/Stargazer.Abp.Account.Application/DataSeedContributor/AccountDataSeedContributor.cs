using Microsoft.Extensions.Localization;
using Stargazer.Abp.Account.Domain.Repository;
using Stargazer.Abp.Account.Domain.Role;
using Stargazer.Abp.Account.Domain.Shared;
using Stargazer.Abp.Account.Domain.Shared.Localization.Resources;
using Stargazer.Abp.Account.Domain.Users;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using static Stargazer.Abp.Account.Application.Contracts.Authorization.AccountPermissions;

namespace Stargazer.Abp.Account.Application.DataSeedContributor;

public class AccountDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IUserRepository _userRepository;
    private readonly IRepository<PermissionData, Guid> _permissionRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly ICurrentTenant _currentTenant;
    private readonly IGuidGenerator _guidGenerator;
    private readonly IStringLocalizer<AccountResource> _localizer;
    public AccountDataSeedContributor(IRepository<PermissionData, Guid> permissionRepository, ICurrentTenant currentTenant, IGuidGenerator guidGenerator, IRoleRepository roleRepository, IUserRepository userRepository, IStringLocalizer<AccountResource> localizer)
    {
        _permissionRepository = permissionRepository;
        _currentTenant = currentTenant;
        _guidGenerator = guidGenerator;
        _roleRepository = roleRepository;
        _userRepository = userRepository;
        _localizer = localizer;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        using (_currentTenant.Change(context?.TenantId))
        {
            await InitPermission();
            await InitRole();
            await InitUser();
        }
    }

    private async Task InitPermission()
    {
        var permissions = await _permissionRepository.GetListAsync();

        {
            var data = permissions.FirstOrDefault(x => x.Permission == User.Manage);
            if (data == null)
            {
                data = new PermissionData(_guidGenerator.Create(), _localizer["UserManagement"], User.Manage);
                _ = await _permissionRepository.InsertAsync(data);
            }

            var createUser = permissions.FirstOrDefault(x => x.Permission == User.Create);
            if (createUser == null)
            {
                createUser = new PermissionData(_guidGenerator.Create(), _localizer["CreateUser"], User.Create, data.Id);
                _ = await _permissionRepository.InsertAsync(createUser);
            }

            var updateUser = permissions.FirstOrDefault(x => x.Permission == User.Update);
            if (updateUser == null)
            {
                updateUser = new PermissionData(_guidGenerator.Create(), _localizer["UpdateUser"], User.Update, data.Id);
                _ = await _permissionRepository.InsertAsync(updateUser);
            }

            var deleteUser = permissions.FirstOrDefault(x => x.Permission == User.Delete);
            if (deleteUser == null)
            {
                deleteUser = new PermissionData(_guidGenerator.Create(), _localizer["DeleteUser"], User.Delete, data.Id);
                _ = await _permissionRepository.InsertAsync(deleteUser);
            }
        }

        {
            var data = permissions.FirstOrDefault(x => x.Permission == Role.Manage);
            if (data == null)
            {
                data = new PermissionData(_guidGenerator.Create(), _localizer["RoleManagement"], Role.Manage);
                _ = await _permissionRepository.InsertAsync(data);
            }

            var createRole = permissions.FirstOrDefault(x => x.Permission == Role.Create);
            if (createRole == null)
            {
                createRole = new PermissionData(_guidGenerator.Create(), _localizer["CreateRole"], Role.Create, data.Id);
                _ = await _permissionRepository.InsertAsync(createRole);
            }

            var updateRole = permissions.FirstOrDefault(x => x.Permission == Role.Update);
            if (updateRole == null)
            {
                updateRole = new PermissionData(_guidGenerator.Create(), _localizer["UpdateRole"], Role.Update, data.Id);
                _ = await _permissionRepository.InsertAsync(updateRole);
            }

            var deleteRole = permissions.FirstOrDefault(x => x.Permission == Role.Delete);
            if (deleteRole == null)
            {
                deleteRole = new PermissionData(_guidGenerator.Create(), _localizer["DeleteRole"], Role.Delete, data.Id);
                _ = await _permissionRepository.InsertAsync(deleteRole);
            }
        }

        {
            var data = permissions.FirstOrDefault(x => x.Permission == Permission.Manage);
            if (data == null)
            {
                data = new PermissionData(_guidGenerator.Create(), _localizer["PermissionManagement"], Permission.Manage);
                _ = await _permissionRepository.InsertAsync(data);
            }

            var createPermission = permissions.FirstOrDefault(x => x.Permission == Permission.Create);
            if (createPermission == null)
            {
                createPermission = new PermissionData(_guidGenerator.Create(), _localizer["CreatePermission"], Permission.Create, data.Id);
                _ = await _permissionRepository.InsertAsync(createPermission);
            }

            var updatePermission = permissions.FirstOrDefault(x => x.Permission == Permission.Update);
            if (updatePermission == null)
            {
                updatePermission = new PermissionData(_guidGenerator.Create(), _localizer["UpdatePermission"], Permission.Update, data.Id);
                _ = await _permissionRepository.InsertAsync(updatePermission);
            }

            var deletePermission = permissions.FirstOrDefault(x => x.Permission == Permission.Delete);
            if (deletePermission == null)
            {
                deletePermission = new PermissionData(_guidGenerator.Create(), _localizer["DeletePermission"], Permission.Delete, data.Id);
                _ = await _permissionRepository.InsertAsync(deletePermission);
            }
        }
    }

    private async Task InitRole()
    {
        string roleName = _localizer["UserCenterManager"];
        var role = await _roleRepository.FindAsync(x => x.Name == roleName);
        if (role == null)
        {
            role = new RoleData(_guidGenerator.Create(), roleName, false, true, true);
            role = await _roleRepository.InsertAsync(role, true);
        }

        role = await RoleAddPermission(Permission.Manage, role);
        role = await RoleAddPermission(Role.Manage, role);
        role = await RoleAddPermission(User.Manage, role);

        _ = await _roleRepository.UpdateAsync(role);
    }

    private async Task<RoleData> RoleAddPermission(string permissionName, RoleData role)
    {
        var permission = await _permissionRepository.GetAsync(x => x.Permission == permissionName);
        var permissions = await _permissionRepository.GetListAsync(x => x.ParentId == permission.Id);

        if (role.Permissions.All(x => x.PermissionId != permission.Id))
        {
            role.Permissions.Add(new RolePermissionData(_guidGenerator.Create(), role.Id, permission.Id));
        }

        foreach (var item in permissions)
        {
            if (role.Permissions.All(x => x.PermissionId != item.Id))
            {
                role.Permissions.Add(new RolePermissionData(_guidGenerator.Create(), role.Id, item.Id));
            }
        }

        return role;
    }

    private async Task InitUser()
    {
        var user = await _userRepository.FindAsync(x => x.Account == "admin");
        if(user == null)
        {
            user = new UserData(_guidGenerator.Create(), "admin", "admin", "admin");
            user.SetPassword("Admin12345678");

            string roleName = _localizer["UserCenterManager"];
            var role = await _roleRepository.GetAsync(x => x.Name == roleName);
            user.AddRole(_guidGenerator.Create(), role.Id);
            _ = await _userRepository.InsertAsync(user);
        }
    }
}