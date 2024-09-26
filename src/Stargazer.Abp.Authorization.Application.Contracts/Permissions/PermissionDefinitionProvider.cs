using Volo.Abp.DependencyInjection;

namespace Stargazer.Abp.Authorization.Application.Contracts.Permissions;

public abstract class PermissionDefinitionProvider : IPermissionDefinitionProvider, ITransientDependency
{
    public virtual void PreDefine()
    {
    }

    public virtual void Define()
    {
    }

    public virtual void PostDefine()
    {
    }
}