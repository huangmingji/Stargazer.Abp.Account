using Volo.Abp.Localization;

namespace Stargazer.Abp.Account.Application.Contracts.Authorization
{
    public class PermissionGroupDefinition 
    {
        public PermissionGroupDefinition(string name, LocalizableString displayName)
        {
            PermissionDefinitionProvider.Permissions.Add(name);
            Name = name;
            DisplayName = displayName.Name;
        }
        
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public List<PermissionDefinition> PermissionDefinitions { get; set; } = new List<PermissionDefinition>();

        public PermissionGroupDefinition AddPermission(string name, LocalizableString displayName)
        {
            PermissionDefinitions.Add(new PermissionDefinition(name, displayName));
            return this;
        }
    }
}