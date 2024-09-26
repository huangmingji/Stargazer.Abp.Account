using System;
using Volo.Abp;

namespace Stargazer.Abp.Authorization.Application.Contracts.Permissions;

public class PermissionDefinitionAlreadyExistsException : BusinessException
{
    public PermissionDefinitionAlreadyExistsException(string message) : base(
        message: message)
    {
    }
}