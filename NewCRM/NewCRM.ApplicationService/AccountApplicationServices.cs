using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Application.Interface;
using NewCRM.Application.Services.Services;
using NewCRM.Domain.DomainSpecification;
using NewCRM.Domain.Entitys.Agent;
using NewCRM.Domain.ValueObject;
using NewCRM.Dto;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Infrastructure.CommonTools.CustomException;

namespace NewCRM.Application.Services
{
    [Export(typeof(IAccountApplicationServices))]
    internal class AccountApplicationServices : BaseService, IAccountApplicationServices
    {
        public AccountDto Login(String accountName, String password)
        {
            ValidateParameter.Validate(accountName).Validate(password);

            var account = AccountContext.Validate(accountName, password).ConvertToDto<Account, AccountDto>();

            UnitOfWork.Commit();

            return account;

        }

        public ConfigDto GetConfig(Int32 accountId)
        {
            ValidateParameter.Validate(accountId);

            var accountResult = GetAccountInfoService(accountId);

            if (accountResult == null)
            {
                throw new BusinessException("该用户可能已被禁用或被删除，请联系管理员");
            }

            var accountConfig = accountResult.Config;

            return DtoConfiguration.ConvertDynamicToDto<ConfigDto>(new
            {
                accountConfig.Desks,
                accountConfig.Id,
                accountConfig.Skin,
                accountConfig.AccountFace,
                accountConfig.AppSize,
                accountConfig.AppVerticalSpacing,
                accountConfig.AppHorizontalSpacing,
                accountConfig.DefaultDeskNumber,
                AppXy = accountConfig.AppXy.ToString().ToLower(),
                DockPosition = accountConfig.DockPosition.ToString().ToLower(),
                WallpaperUrl = accountConfig.Wallpaper.Url,
                WallpaperWidth = accountConfig.Wallpaper.Width,
                WallpaperHeigth = accountConfig.Wallpaper.Height,
                WallpaperSource = accountConfig.Wallpaper.Source.ToString().ToLower(),
                WallpaperMode = accountConfig.WallpaperMode.ToString().ToLower()
            });

        }

        public List<AccountDto> GetAccounts(String accountName, String accountType, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
        {
            ValidateParameter.Validate(accountName).Validate(pageIndex).Validate(pageSize);

            var specification = SpecificationFactory.Create<Account>(account => (accountName + "").Length == 0 || account.Name.Contains(accountName));

            AccountType internalAccountType;

            if ((accountType + "").Length > 0)
            {
                var enumConst = Enum.GetName(typeof(AccountType), accountType);

                if (Enum.TryParse(enumConst, true, out internalAccountType))
                {
                    specification.And(account => account.IsAdmin == (internalAccountType == AccountType.Admin));
                }
                else
                {
                    throw new BusinessException($"用户类型{accountType}不是有效的类型");
                }
            }

            return Query.PageBy(specification, pageIndex, pageSize, out totalCount).Select(account => new
            {
                account.Id,
                AccountType = account.IsAdmin ? "2" /*管理员*/ : "1" /*用户*/,
                account.Name
            }).ConvertDynamicToDtos<AccountDto>().ToList();
          
        }

        public AccountDto GetAccount(Int32 accountId)
        {
            ValidateParameter.Validate(accountId);

            var accountResult = GetAccountInfoService(accountId);

            if (accountResult == null)
            {
                throw new BusinessException("该用户可能已被禁用或被删除，请联系管理员");
            }

            return DtoConfiguration.ConvertDynamicToDto<AccountDto>(new
            {
                accountResult.Id,
                accountResult.Name,
                Password = accountResult.LoginPassword,
                AccountType = accountResult.IsAdmin ? "2" : "1",
                Roles = accountResult.AccountRoles.Select(s => new
                {
                    Id = s.RoleId
                })
            });

        }

        public void AddNewAccount(AccountDto accountDto)
        {
            ValidateParameter.Validate(accountDto);

            var account = accountDto.ConvertToModel<AccountDto, Account>();

            AccountType accountType;

            var enumConst = Enum.GetName(typeof(AccountType), account.IsAdmin ? "2"/*管理员*/ : "1"/*用户*/);

            if (!Enum.TryParse(enumConst, true, out accountType))
            {
                throw new BusinessException($"类型{enumConst}不是有效的枚举类型");
            }

            var internalNewAccount = new Account(account.Name, PasswordUtil.CreateDbPassword(account.LoginPassword), accountType);

            internalNewAccount.AddRole(account.AccountRoles.Select(role => role.RoleId).ToArray());

            Repository.Create<Account>().Add(internalNewAccount);

            UnitOfWork.Commit();
        }

        public Boolean CheckAccountNameExist(String accountName)
        {
            ValidateParameter.Validate(accountName);

            return Query.Find(SpecificationFactory.Create<Account>(account => account.Name == accountName)).Any();

        }

        public void ModifyAccount(AccountDto accountDto)
        {
            ValidateParameter.Validate(accountDto);

            var account = accountDto.ConvertToModel<AccountDto, Account>();

            var accountResult = Query.FindOne(SpecificationFactory.Create<Account>(internalAccount => internalAccount.Id == account.Id));

            if (accountResult == null)
            {
                throw new BusinessException($"用户{account.Name}可能已被禁用或删除");
            }
            
            if ((account.LoginPassword + "").Length > 0)
            {
                var newPassword = PasswordUtil.CreateDbPassword(account.LoginPassword);
                accountResult.ModifyPassword(newPassword);
            }

            if (accountResult.AccountRoles.Any())
            {
                accountResult.AccountRoles.ToList().ForEach(role =>
                {
                    role.Remove();
                });
            }

            accountResult.AddRole(account.AccountRoles.Select(role => role.RoleId).ToArray());

            Repository.Create<Account>().Update(accountResult);

            UnitOfWork.Commit();

        }

        public void Logout(Int32 accountId)
        {
            ValidateParameter.Validate(accountId);

            AccountContext.Logout(accountId);

            UnitOfWork.Commit();
        }

        public void Enable(Int32 accountId)
        {
            ValidateParameter.Validate(accountId);

            var accountResult = GetAccountInfoService(accountId);

            if (accountResult == null)
            {
                throw new BusinessException("该用户可能已被禁用或被删除，请联系管理员");
            }

            accountResult.Enable();

            Repository.Create<Account>().Update(accountResult);

            UnitOfWork.Commit();

        }

        public void Disable(Int32 accountId)
        {
            ValidateParameter.Validate(accountId);

            var accountResult = GetAccountInfoService(accountId);

            if (accountResult == null)
            {
                throw new BusinessException("该用户可能已被禁用或被删除，请联系管理员");
            }

            accountResult.Disable();

            Repository.Create<Account>().Update(accountResult);

            UnitOfWork.Commit();
        }
    }
}
