using System;
using Volo.Abp;

namespace Stargazer.Abp.Account.Domain.Role
{
    public class RoleNotNullException : BusinessException
    {
        public RoleNotNullException(Guid? roleId, string roleName)
            : base(message: $"The role ({roleId}) name ({roleName}) already exists)")
        {

        }
    }
}