namespace Stargazer.Abp.Authorization.Application.Contracts.Permissions;

public static class PermissionGroupDefinition
{
    private static List<PermissionDefinition> PermissionDefinitions { get; set; } = new List<PermissionDefinition>();

    public static PermissionDefinition CreateGroup(string name, string displayName, string description = "")
    {
        var group = PermissionDefinitions.FirstOrDefault(x => x.ParentName == "" && x.Name == name);
        if (group != null)
        {
            return group;
        }
        group = new PermissionDefinition(name, displayName, "", description);
        PermissionDefinitions.Add(group);
        return group;
    }

    /// <summary>
    /// 向已有的权限定义parent中添加一个新的权限定义,返回创建的新权限定义对象。
    /// </summary>
    /// <param name="group"></param>
    /// <param name="name"></param>
    /// <param name="displayName"></param>
    /// <param name="description"></param>
    /// <returns>返回创建的新权限定义对象。</returns>
    /// <exception cref="PermissionDefinitionAlreadyExistsException"></exception>
    public static PermissionDefinition AddPermission(this PermissionDefinition group, string name, string displayName, string description = "")
    {
        var permissionDefinition = PermissionDefinitions.FirstOrDefault(x => x.ParentName == group.Name && x.Name == name);
        if (permissionDefinition != null)
        {
            return permissionDefinition;
        }
        permissionDefinition = new PermissionDefinition(name, displayName, group.Name, description);
        PermissionDefinitions.Add(permissionDefinition);
        return permissionDefinition;
    }

    /// <summary>
    /// 向已有的权限定义permisson中添加子权限
    /// </summary>
    /// <param name="permisson"></param>
    /// <param name="name"></param>
    /// <param name="displayName"></param>
    /// <param name="description"></param>
    /// <returns>返回原始权限定义对象</returns>
    /// <exception cref="PermissionDefinitionAlreadyExistsException"></exception>
    public static PermissionDefinition AddChild(this PermissionDefinition permisson, string name, string displayName, string description = "")
    {
        var child = PermissionDefinitions.FirstOrDefault(x => x.ParentName == permisson.Name && x.Name == name);
        if (child != null)
        {
            return permisson;
        }
        child = new PermissionDefinition(name, displayName, permisson.Name, description);
        PermissionDefinitions.Add(child);
        return permisson;
    }

    public static List<PermissionDefinition> GetGroups()
    {
        return PermissionDefinitions.Where(x => x.ParentName == "").ToList();
    }

    public static List<PermissionDefinition> GetAllPermissions()
    {
        return PermissionDefinitions.Where(x => x.ParentName != "").ToList();
    }

    public static List<PermissionDefinition> GetChildren(string parentName)
    {
        return PermissionDefinitions.Where(x => x.ParentName == parentName).ToList();
    }
}