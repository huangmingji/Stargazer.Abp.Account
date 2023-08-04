using System;
using System.Collections.Generic;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;
using static Lemon.Common.Cryptography;

namespace Stargazer.Abp.Account.Domain.Users
{
    /// <summary>
    /// 用户数据
    /// </summary>
    public sealed class UserData : AuditedAggregateRoot<Guid>, ISoftDelete
    {
        /// <summary>
        /// 账号
        /// </summary>
        /// <value>The account.</value>
        public string Account { get; protected set; } = "";

        /// <summary>
        /// 昵称
        /// </summary>
        /// <value>The name of the nike.</value>
        public string NickName { get; protected set; } = "";

        /// <summary>
        /// 头像
        /// </summary>
        /// <value>The head icon.</value>
        public string HeadIcon { get; protected set; } = "";

        /// <summary>
        /// 手机号码
        /// </summary>
        /// <value>The phone number.</value>
        public string? PhoneNumber { get; protected set; }

        /// <summary>
        /// 手机号码是否已验证
        /// </summary>
        /// <value></value>
        public bool PhoneNumberVerified { get; protected set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        /// <value>The email.</value>
        public string? Email { get; protected set; }

        /// <summary>
        /// 电子邮箱是否已验证
        /// </summary>
        /// <value></value>
        public bool EmailVerified { get; protected set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; protected set; } = "";

        /// <summary>
        /// 用户密钥
        /// </summary>
        public string SecretKey { get; protected set; } = "";

        /// <summary>
        /// 允许同时有多用户登录
        /// </summary>
        public bool MultiUserLogin { get; protected set; }

        /// <summary>
        /// 登录次数
        /// </summary>
        public int LogonCount { get; protected set; }

        /// <summary>
        /// 在线状态
        /// </summary>
        public bool UserOnline { get; protected set; } = false;

        /// <summary>
        /// 允许登录时间开始
        /// </summary>
        public DateTime AllowStartTime { get; protected set; } = DateTime.Now;

        /// <summary>
        /// 允许登录时间结束
        /// </summary>
        public DateTime AllowEndTime { get; protected set; } = DateTime.Now.AddYears(100);

        /// <summary>
        /// 暂停用户开始日期
        /// </summary>
        public DateTime LockStartTime { get; protected set; } = DateTime.Now.AddYears(100);

        /// <summary>
        /// 暂停用户结束日期
        /// </summary>
        public DateTime LockEndDate { get; protected set; } = DateTime.Now.AddYears(100);

        /// <summary>
        /// 第一次访问时间
        /// </summary>
        public DateTime FirstVisitTime { get; protected set; } = DateTime.Now;

        /// <summary>
        /// 上一次访问时间
        /// </summary>
        public DateTime PreviousVisitTime { get; protected set; } = DateTime.Now;

        /// <summary>
        /// 最后访问时间
        /// </summary>
        public DateTime LastVisitTime { get; protected set; } = DateTime.Now;

        /// <summary>
        /// 最后修改密码日期
        /// </summary>
        public DateTime ChangPasswordDate { get; protected set; } = DateTime.Now;

        public List<UserRole> UserRoles = new List<UserRole>();

        public UserData()
        {
        }

        public UserData(Guid id, string password, string name)
            : this(id, null, password, name)
        {
        }

        public UserData(Guid id, string account, string password, string name = "", string phoneNumber = null)
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));
            Check.NotNullOrWhiteSpace(password, nameof(password));

            Id = id;
            NickName = name;
            Account = string.IsNullOrWhiteSpace(account) ? Guid.NewGuid().ToString("N") : account;
            PhoneNumber = phoneNumber;
            Password = PasswordStorage.CreateHash(password, out string secretKey);
            SecretKey = secretKey;
        }

        public void SetHeadIcon(string headIcon)
        {
            HeadIcon = headIcon;
        }

        public void AddRole(Guid id, Guid roleId)
        {
            UserRoles.Add(new UserRole(id, this.Id, roleId));
        }

        public void SetRoles(Dictionary<Guid, Guid> roleIds)
        {
            UserRoles.Clear();
            foreach (var item in roleIds)
            {
                UserRoles.Add(new UserRole(item.Key, this.Id, item.Value));
            }
        }

        public void Set(string name, string account, string email, string phoneNumber)
        {
            Check.NotNullOrEmpty(name, nameof(name));
            Check.NotNullOrEmpty(account, nameof(account));
            Check.NotNullOrEmpty(email, nameof(email));
            Check.NotNullOrEmpty(phoneNumber, nameof(phoneNumber));

            this.NickName = name;
            this.Account = account;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
        }

        public void SetAccount(string account)
        {
            Check.NotNullOrEmpty(account, nameof(account));
            Account = account;
        }

        public void SetName(string name)
        {
            Check.NotNullOrEmpty(name, nameof(name));
            NickName = name;
        }

        public void SetPassword(string password)
        {
            string passwordHash = PasswordStorage.CreateHash(password, out string secretKey);
            Password = passwordHash;
            SecretKey = secretKey;
            ChangPasswordDate = DateTime.Now;
        }

        public void SetEmail(string email, bool emailVerified)
        {
            Email = email;
            EmailVerified = emailVerified;
            if (!emailVerified)
            {
                AddLocalEvent(new EmailChangedEvent(email));
            }
        }

        public void SetPhoneNumber(string phoneNumber, bool phoneNumberVerified)
        {
            PhoneNumber = phoneNumber;
            PhoneNumberVerified = phoneNumberVerified;
        }

        public void UpdateLoginInfo()
        {
            FirstVisitTime = LogonCount == 0 ? DateTime.Now : FirstVisitTime;
            LogonCount += 1;
            PreviousVisitTime = LastVisitTime;
            LastVisitTime = DateTime.Now;
        }

        public void VerifyPassword(string password)
        {
            if (!PasswordStorage.VerifyPassword(password, Password, SecretKey))
            {
                throw new VerifyPasswordException(base.Id, password);
            }
        }

        public void LockUser(DateTime startTime, DateTime endTime)
        {
            LockStartTime = startTime;
            LockEndDate = endTime;
        }

        public void AllowUser(DateTime startTime, DateTime endTime)
        {
            AllowStartTime = startTime;
            AllowEndTime = endTime;
        }

        public bool IsDeleted { get; set; }

        public void VerifiedPhoneNumber(string phoneNumber)
        {
            if (PhoneNumber != phoneNumber)
            {
                throw new VerifiedPhoneNumberException(Id, phoneNumber);
            }

            this.PhoneNumberVerified = true;
        }

        public void VerifiedEmail(string email)
        {
            if (Email != email)
            {
                throw new VerifiedEmailException(Id, email);
            }

            this.EmailVerified = true;
        }
    }
}