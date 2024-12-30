using System;
using Volo.Abp;

namespace Stargazer.Abp.Account.Domain.Role
{
    public class RoleNotNullException : UserFriendlyException
    {
        public RoleNotNullException(Guid? roleId, string roleName)
            : base(message: "该角色已存在！", details: $"The role ({roleId}) name ({roleName}) already exists)")
        {

        }
    }
}