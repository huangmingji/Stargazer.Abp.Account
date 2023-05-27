using System.ComponentModel.DataAnnotations;
using Shouldly;
using Stargazer.Abp.Account.Domain.Role;

namespace Stargazer.Abp.Account.Domain.Tests;

public class RoleTest
{
    [Fact]
    public void Test()
    {
        var permissionParent = new PermissionData(Guid.NewGuid(), "用户管理", "Stargazer.Abp.Account.User");
        permissionParent.Name.ShouldBeEquivalentTo("用户管理");
        permissionParent.Permission.ShouldBeEquivalentTo("Stargazer.Abp.Account.User");
        permissionParent.ParentId.ShouldBeNull();

        var permissionChild = new PermissionData(Guid.NewGuid(), "创建用户", "Stargazer.Abp.Account.User.Create", permissionParent.Id);
        permissionChild.Name.ShouldBeEquivalentTo("创建用户");
        permissionChild.Permission.ShouldBeEquivalentTo("Stargazer.Abp.Account.User.Create");
        permissionChild.ParentId.ShouldBeEquivalentTo(permissionParent.Id);

        var role = new RoleData(Guid.NewGuid(), "超级管理员", false, true, false);
        role.Name.ShouldBeEquivalentTo("超级管理员");
        role.IsDefault.ShouldBeFalse();
        role.IsStatic.ShouldBeTrue();
        role.IsPublic.ShouldBeFalse();

        var role1 = new RoleData(Guid.NewGuid(), "超级管理员", true, false, true);
        role1.Name.ShouldBeEquivalentTo("超级管理员");
        role1.IsDefault.ShouldBeTrue();
        role1.IsStatic.ShouldBeFalse();
        role1.IsPublic.ShouldBeTrue();

        var rolePermissionData = new RolePermissionData(Guid.NewGuid(), role.Id, "Stargazer.Abp.Account.User.Create");
        rolePermissionData.RoleId.ShouldBeEquivalentTo(role.Id);
        rolePermissionData.Permission.ShouldBeEquivalentTo("Stargazer.Abp.Account.User.Create");
    }
}