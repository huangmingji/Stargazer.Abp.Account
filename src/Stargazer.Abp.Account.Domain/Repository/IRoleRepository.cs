using System;
using System.Threading.Tasks;
using Stargazer.Abp.Account.Domain.Role;
using Volo.Abp.Domain.Repositories;

namespace Stargazer.Abp.Account.Domain.Repository
{
    public interface IRoleRepository: IRepository<RoleData, Guid>
    {
        Task CheckNotNull(string name, Guid? id = null);
    }
}