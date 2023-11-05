using Volo.Abp.Localization;

namespace  Stargazer.Abp.Account.Application.Contracts.Authorization
{
    public class PermissionDefinitionProvider
    {
        public static List<string> Permissions { get; set;} = new List<string>();
        public static List<PermissionGroupDefinition> PermissionGroupDefinitions{ get; set;} = new List<PermissionGroupDefinition>();

        public static PermissionGroupDefinition AddGroup(string name, LocalizableString displayName)
        {
            PermissionGroupDefinition permissionGroupDefinition = new PermissionGroupDefinition(name, displayName); 
            PermissionGroupDefinitions.Add(permissionGroupDefinition);
            return permissionGroupDefinition;
        }
    }
}