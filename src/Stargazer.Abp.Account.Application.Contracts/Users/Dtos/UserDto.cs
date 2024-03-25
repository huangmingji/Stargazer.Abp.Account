using Stargazer.Abp.Account.Domain.Shared.Users;

namespace Stargazer.Abp.Account.Application.Contracts.Users.Dtos
{

    /// <summary>
    /// 用户数据
    /// </summary>
    public class UserDto
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        /// <value>The account.</value>
        public string Account { get; set; } = "";

        /// <summary>
        /// 昵称
        /// </summary>
        /// <value>The name of the nike.</value>
        public string NickName { get; set; } = "";

        /// <summary>
        /// 头像
        /// </summary>
        /// <value>The head icon.</value>
        public string HeadIcon { get; set; } = "";

        /// <summary>
        /// 手机号码
        /// </summary>
        /// <value>The phone number.</value>
        public string PhoneNumber { get; set; } = "";

        /// <summary>
        /// 手机号码是否已验证
        /// </summary>
        /// <value></value>
        public bool PhoneNumberVerified { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        /// <value>The email.</value>
        public string Email { get; set; } = "";

        /// <summary>
        /// 电子邮箱是否已验证
        /// </summary>
        /// <value></value>
        public bool EmailVerified { get; set; }

        /// <summary>
        /// 允许同时有多用户登录
        /// </summary>
        public bool MultiUserLogin { get; set; }

        /// <summary>
        /// 登录次数
        /// </summary>
        public int LogonCount { get; set; }

        /// <summary>
        /// 在线状态
        /// </summary>
        public bool UserOnline { get; set; } = false;

        /// <summary>
        /// 允许登录时间开始
        /// </summary>
        public DateTime AllowStartTime { get; set; } = DateTime.Now.Date.AddDays(-1);

        /// <summary>
        /// 允许登录时间结束
        /// </summary>
        public DateTime AllowEndTime { get; set; } = DateTime.Now.Date.AddYears(100);

        /// <summary>
        /// 暂停用户开始日期
        /// </summary>
        public DateTime LockStartTime { get; set; } = DateTime.Now.Date.AddYears(100);

        /// <summary>
        /// 暂停用户结束日期
        /// </summary>
        public DateTime LockEndDate { get; set; } = DateTime.Now.Date.AddYears(100);

        /// <summary>
        /// 第一次访问时间
        /// </summary>
        public DateTime FirstVisitTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 上一次访问时间
        /// </summary>
        public DateTime PreviousVisitTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 最后访问时间
        /// </summary>
        public DateTime LastVisitTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 最后修改密码日期
        /// </summary>
        public DateTime ChangPasswordDate { get; set; } = DateTime.Now;
        
        public List<UserRoleDto> UserRoles { get; set; } = new List<UserRoleDto>();

        public List<string> Permissions => GetPermissions();
        
        public List<string> GetPermissions(Guid? tenantId = null)
        {
            var permissions = new List<string>();
            if (tenantId == null)
            {
                tenantId = UserRoles.FirstOrDefault()?.TenantId;
            }

            foreach (var userRoleDto in UserRoles.FindAll(x => x.TenantId == tenantId))
            {
                if (userRoleDto.RoleData != null)
                {
                    permissions.AddRange(userRoleDto.RoleData.Permissions.ConvertAll(x => x.PermissionData.Permission));
                }
            }
            return permissions;
        }
        
        public void CheckAllowTime()
        {
            DateTime now = DateTime.Now;
            if (now < AllowStartTime || now > AllowEndTime)
            {
                throw new UserNotAllowLoginException(Id);
            }
        }

        public void CheckLockTime()
        {
            DateTime now = DateTime.Now;
            if (now > LockStartTime && now < LockEndDate)
            {
                throw new UserLockLoginException(Id);
            }
        }
    }
}