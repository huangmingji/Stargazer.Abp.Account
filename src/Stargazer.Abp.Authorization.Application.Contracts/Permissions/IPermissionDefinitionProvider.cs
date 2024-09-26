namespace Stargazer.Abp.Authorization.Application.Contracts.Permissions;

public interface IPermissionDefinitionProvider
{
    void PreDefine();

    void Define();

    void PostDefine();
}