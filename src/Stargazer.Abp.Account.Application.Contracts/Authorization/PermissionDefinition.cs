using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Stargazer.Abp.Account.Application.Contracts.Authorization
{
    public class PermissionDefinition 
    {
        public PermissionDefinition(string name, LocalizableString displayName)
        {
            PermissionDefinitionProvider.Permissions.Add(name);
            Name = name;
            DisplayName = displayName.Name;
        }

        public string Name { get; set; }

        public string DisplayName { get; set; }
    }
}