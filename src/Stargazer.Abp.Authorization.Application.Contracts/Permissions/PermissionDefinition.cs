namespace Stargazer.Abp.Authorization.Application.Contracts.Permissions;

public class PermissionDefinition
{
    public PermissionDefinition(string name, string displayName, string parentName = "", string description = "")
    {
        ParentName = parentName;
        Name = name;
        DisplayName = displayName;
        Description = description;
    }

    public string ParentName { get; set; }

    public string Name { get; set;}

    public string DisplayName { get; set; }

    public string Description { get; set; }

}