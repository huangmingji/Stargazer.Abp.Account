﻿using System;
using Stargazer.Common.Extend;
using Shouldly;
using Stargazer.Abp.Account.Domain.Users;

namespace Stargazer.Abp.Account.Domain.Tests;

public class UserTest
{
    [Fact]
    public void Test()
    {
        var user = new UserData(Guid.NewGuid(), "root");
        user.SetEmail("123456@qq.com", true);
        user.NickName.ShouldBeEquivalentTo("root");
        user.Email.ShouldBeEquivalentTo("123456@qq.com");
        user.SetPassword("dfdkUDFdfskfj34234234");
        user.VerifyPassword("dfdkUDFdfskfj34234234");
        user.AllowUser(DateTime.Now, DateTime.Now.AddMinutes(2));
        user.CheckAllowTime();
        user.LockUser(DateTime.Now.AddMinutes(1), DateTime.Now.AddMinutes(2));
        user.CheckLockTime();

        var user1 = new UserData(Guid.NewGuid(), "admin", "admin", "1234567890");
        user1.SetPassword("dfdkUDFdfskfj34234234");
        user1.SetEmail("2222@qq.com", true);

        user1.NickName.ShouldBeEquivalentTo("admin");
        user1.Email.ShouldBeEquivalentTo("2222@qq.com");
        user1.PhoneNumber.ShouldBeEquivalentTo("1234567890");
        user1.VerifyPassword("dfdkUDFdfskfj34234234");

        user.SetAccount("anyone");
        user.Account.ShouldBeEquivalentTo("anyone");

        user.SetEmail("32323@gmail.com", false);
        user.Email.ShouldBeEquivalentTo("32323@gmail.com");
        user.EmailVerified.ShouldBeFalse();
        user.SetEmail("32323@outlook.com", true);
        user.Email.ShouldBeEquivalentTo("32323@outlook.com");
        user.EmailVerified.ShouldBeTrue();

        user.SetName("Bill");
        user.NickName.ShouldBeEquivalentTo("Bill");

        user.SetPassword("fdfdHG434343434");
        user.VerifyPassword("fdfdHG434343434");

        user.SetPhoneNumber("0987654321", false);
        user.PhoneNumber.ShouldBeEquivalentTo("0987654321");
        user.PhoneNumberVerified.ShouldBeFalse();
        user.SetPhoneNumber("098765432111", true);
        user.PhoneNumber.ShouldBeEquivalentTo("098765432111");
        user.PhoneNumberVerified.ShouldBeTrue();

        var lockStartTime = DateTime.Now;
        var lockEndTime = lockStartTime.AddDays(2);
        user.LockUser(lockStartTime, lockEndTime);
        user.LockStartTime.ShouldBeEquivalentTo(lockStartTime);
        user.LockEndDate.ShouldBeEquivalentTo(lockEndTime);

        var allowStartTime = DateTime.Now.AddDays(1);
        var allowEndTime = allowStartTime.AddDays(3);
        user.AllowUser(allowStartTime, allowEndTime);
        user.AllowStartTime.ShouldBeEquivalentTo(allowStartTime);
        user.AllowEndTime.ShouldBeEquivalentTo(allowEndTime);

        user.SetData("tom", "tom1", "2290@163.com", "1029384756");
        user.Account.ShouldBeEquivalentTo("tom1");
        user.NickName.ShouldBeEquivalentTo("tom");
        user.Email.ShouldBeEquivalentTo("2290@163.com");
        user.PhoneNumber.ShouldBeEquivalentTo("1029384756");

        user.SetAddress("中国", "广东省", "zhognshan", "东区", "中山一路");
        user.Country.ShouldBeEquivalentTo("中国");
        user.Province.ShouldBeEquivalentTo("广东省");
        user.City.ShouldBeEquivalentTo("zhognshan");
        user.District.ShouldBeEquivalentTo("东区");
        user.Address.ShouldBeEquivalentTo("中山一路");
    }
}