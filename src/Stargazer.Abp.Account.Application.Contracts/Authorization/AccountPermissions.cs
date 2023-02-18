using System.Collections.Generic;
using Stargazer.Abp.Account.Application.Contracts.Permissions.Dtos;
using Volo.Abp.Reflection;

namespace Stargazer.Abp.Account.Application.Contracts.Authorization
{
    public class AccountPermissions
    {
        private const string GroupName = "Stargazer.Abp.Account";

        public static class User
        {
            private const string Default = GroupName + ".User";
            public const string Manage = Default + ".Manage";
            public const string Delete = Default + ".Delete";
            public const string Update = Default + ".Update";
            public const string Create = Default + ".Create";
        }

        public static class Role
        {
            private const string Default = GroupName + ".Role";
            public const string Manage = Default + ".Manage";
            public const string Delete = Default + ".Delete";
            public const string Update = Default + ".Update";
            public const string Create = Default + ".Create";
        }

        public static class Permission
        {
            private const string Default = GroupName + ".Permission";
            public const string Manage = Default + ".Manage";
            public const string Delete = Default + ".Delete";
            public const string Update = Default + ".Update";
            public const string Create = Default + ".Create";
        }

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(AccountPermissions));
        }

        public static List<PermissionDto> DefaultPermissions()
        {
            List<PermissionDto> permissionDtos = new List<PermissionDto>();

            #region User
            
            var user = new PermissionDto()
            {
                Name = "用户管理",
                Permission = User.Manage
            };
            user.Permissions.Add(new PermissionDto()
            {
                Name = "新增",
                Permission = User.Create
            });
            user.Permissions.Add(new PermissionDto()
            {
                Name = "删除",
                Permission = User.Delete
            });
            user.Permissions.Add(new PermissionDto()
            {
                Name = "修改",
                Permission = User.Update
            });
            permissionDtos.Add(user);

            #endregion

            #region Role

            var role = new PermissionDto()
            {
                Name = "角色管理",
                Permission = Role.Manage
            };
            role.Permissions.Add(new PermissionDto()
            {
                Name = "新增",
                Permission = Role.Create
            });
            role.Permissions.Add(new PermissionDto()
            {
                Name = "删除",
                Permission = Role.Delete
            });
            role.Permissions.Add(new PermissionDto()
            {
                Name = "修改",
                Permission = Role.Update
            });
            permissionDtos.Add(role);

            #endregion

            #region Permission

            var permission = new PermissionDto()
            {
                Name = "权限管理",
                Permission = Permission.Manage
            };
            permission.Permissions.Add(new PermissionDto()
            {
                Name = "新增",
                Permission = Permission.Create
            });
            permission.Permissions.Add(new PermissionDto()
            {
                Name = "删除",
                Permission = Permission.Delete
            });
            permission.Permissions.Add(new PermissionDto()
            {
                Name = "修改",
                Permission = Permission.Update
            });
            permissionDtos.Add(permission);

            #endregion
            
            return permissionDtos;
        }
    }
}