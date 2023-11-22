using FluentValidation;
using Stargazer.Abp.Account.Application.Contracts.Permissions.Dtos;
using Stargazer.Abp.Account.Application.Contracts.Roles.Dtos;
using Stargazer.Abp.Account.Application.Contracts.Users.Dtos;

namespace Stargazer.Abp.Account.Application.Contracts
{
    public class VerifyPasswordDtoValidator : AbstractValidator<VerifyPasswordDto>
    {
        public VerifyPasswordDtoValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("账号密码不能为空");
            RuleFor(x => x.Password).NotNull().NotEmpty().WithMessage("账号密码不能为空");
        }
    }

    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(x=> x.UserName).NotNull().NotEmpty().WithMessage("用户名不能为空");
            RuleFor(x=> x.Email).NotNull().NotEmpty().WithMessage("邮箱地址不能为空");
            RuleFor(x=> x.Email).EmailAddress().WithMessage("请输入一个邮箱地址");
            RuleFor(x=> x.Password).NotNull().NotEmpty().WithMessage("密码不能为空");
            RuleFor(x=>x.Password).Matches(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}$").WithMessage("密码必须包含大小写字母和数字，且长度8位以上");
        }
    }
    
    public class CreateOrUpdateUserWithRolesDtoValidator : AbstractValidator<CreateOrUpdateUserWithRolesDto>
    {
        public CreateOrUpdateUserWithRolesDtoValidator()
        {            
            RuleFor(x=> x.Account).NotNull().NotEmpty().WithMessage("账号不能为空");
            RuleFor(x=> x.UserName).NotNull().NotEmpty().WithMessage("用户名不能为空");
            RuleFor(x=> x.Email).EmailAddress().WithMessage("请输入一个邮箱地址");
        }
    }

    public class UpdatePasswordDtoValidator : AbstractValidator<UpdatePasswordDto>
    {
        public UpdatePasswordDtoValidator()
        {
            RuleFor(x=> x.Password).NotNull().NotEmpty().WithMessage("密码不能为空");
            RuleFor(x=>x.Password).Matches(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}$").WithMessage("请输入至少8个字符，由大小写字母、数字组合而成的密码");
        }
    }
    
    public class UpdateUserPasswordDtoValidator : AbstractValidator<UpdateUserPasswordDto>
    {
        public UpdateUserPasswordDtoValidator()
        {
            RuleFor(x=> x.OldPassword).NotNull().NotEmpty().WithMessage("当前密码不能为空");
            RuleFor(x=> x.Password).NotNull().NotEmpty().WithMessage("密码不能为空");
            RuleFor(x=>x.Password).Matches(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}$").WithMessage("请输入至少8个字符，由大小写字母、数字组合而成的密码");
        }
    }
    
    public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserDtoValidator()
        {
            RuleFor(x=>x.Name).NotNull().NotEmpty().WithMessage("姓名不能为空");
            RuleFor(x=>x.Account).NotNull().NotEmpty().WithMessage("账号不能为空");
            RuleFor(x=>x.PhoneNumber).NotNull().NotEmpty().WithMessage("手机号码不能为空");
        }
    }

    public class  UpdatePermissionDtoValidator : AbstractValidator<UpdatePermissionDto>
    {
        public UpdatePermissionDtoValidator()
        {
            RuleFor(x=>x.Name).NotNull().NotEmpty().WithMessage("名称不能为空");
            RuleFor(x=>x.Permission).NotNull().NotEmpty().WithMessage("权限不能为空");
        }
    }

    public class UpdateRoleDtoValidator : AbstractValidator<UpdateRoleDto>
    {
        public UpdateRoleDtoValidator()
        {
            RuleFor(x=> x.Name).NotNull().NotEmpty().WithMessage("名称不能为空");
        }
    }

    public class UpdateUserNameDtoValidator: AbstractValidator<UpdateUserNameDto>
    {
        public UpdateUserNameDtoValidator()
        {
            RuleFor(x=> x.Name).NotNull().NotEmpty().WithMessage("名称不能为空");
        }
    }
    
    public class UpdateEmailDtoValidator: AbstractValidator<UpdateEmailDto>
    {
        public UpdateEmailDtoValidator()
        {
            RuleFor(x=> x.Email).NotNull().NotEmpty().WithMessage("电子邮件不能为空");
        }
    }

    public class UpdatePhoneNumberDtoValidator: AbstractValidator<UpdatePhoneNumberDto>
    {
        public UpdatePhoneNumberDtoValidator()
        {
            RuleFor(x=> x.PhoneNumber).NotNull().NotEmpty().WithMessage("手机号码不能为空");
        }
    }

    public class UpdateAccountDtoValidator: AbstractValidator<UpdateAccountDto>
    {
        public UpdateAccountDtoValidator()
        {
            RuleFor(x=> x.Account).NotNull().NotEmpty().WithMessage("账号不能为空");
        }
    }
}