using System;
using AutoMapper;
using Stargazer.Abp.Account.Application.Contracts.Permissions.Dtos;
using Stargazer.Abp.Account.Application.Contracts.Roles.Dtos;
using Stargazer.Abp.Account.Application.Contracts.Users.Dtos;
using Stargazer.Abp.Account.Domain.Role;
using Stargazer.Abp.Account.Domain.Users;
using Volo.Abp.AutoMapper;

namespace Stargazer.Abp.Account.Application
{
    public class StargazerAbpAccountApplicationAutoMapperProfile : Profile
    {
        public StargazerAbpAccountApplicationAutoMapperProfile()
        {
            CreateUserDataMappings();
        }
        
        private void CreateUserDataMappings()
        {
            CreateMap<UserData, UserDto>().Ignore(x=>x.Permissions);
            CreateMap<UserRole, UserRoleDto>();
            CreateMap<RoleData, RoleDto>();
            CreateMap<RolePermissionData, RolePermissionDto>();
            CreateMap<PermissionData, PermissionDto>().Ignore(x => x.Permissions);
        }
    }
}