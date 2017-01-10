using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Application.Interface;
using NewCRM.Application.Services.Services;
using NewCRM.Domain.DomainSpecification;
using NewCRM.Domain.Entitys.Agent;
using NewCRM.Domain.Entitys.System;
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

        public ConfigDto GetConfig()
        {

            var accountResult = Query.FindOne((Account account) => account.Id == AccountId);

            if (accountResult == null)
            {
                throw new BusinessException("该用户可能已被禁用或被删除，请联系管理员");
            }

            var accountConfig = accountResult.Config;

            return DtoConfiguration.ConvertDynamicToDto<ConfigDto>(new
            {
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

            var specification = FilterFactory.Create<Account>(account => (accountName + "").Length == 0 || account.Name.Contains(accountName));

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
                account.Name,
                account.Config.AccountFace
            }).ConvertDynamicToDtos<AccountDto>().ToList();

        }

        public AccountDto GetAccount(Int32 accountId = 0)
        {
            Account accountResult = null;
            if (accountId == 0)
            {
                accountResult = Query.FindOne((Account account) => account.Id == AccountId);
            }
            else
            {
                accountResult = Query.FindOne((Account account) => account.Id == accountId);
            }


            if (accountResult == null)
            {
                throw new BusinessException("该用户可能已被禁用或被删除，请联系管理员");
            }

            return accountResult.ConvertToDto<Account, AccountDto>();

        }

        public Boolean CheckAccountNameExist(String accountName)
        {
            ValidateParameter.Validate(accountName);

            return !Query.Find(FilterFactory.Create<Account>(account => account.Name == accountName)).Any();

        }

        public Boolean CheckPassword(String oldAccountPassword)
        {
            ValidateParameter.Validate(oldAccountPassword);

            var accountResult = Query.FindOne(FilterFactory.Create((Account account) => account.Id == AccountId));

            return PasswordUtil.ComparePasswords(accountResult.LoginPassword, oldAccountPassword);
        }

        public IEnumerable<DeskDto> GetDesks()
        {
            return Query.Find((Desk desk) => desk.AccountId == AccountId).ConvertToDtos<Desk, DeskDto>();
        }



        public void AddNewAccount(AccountDto accountDto)
        {
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

            IList<Desk> desks = new List<Desk>();

            for (int i = 1; i <= 5; i++)
            {
                desks.Add(new Desk(i, internalNewAccount.Id));
            }

            Repository.Create<Desk>().Add(desks);

            UnitOfWork.Commit();
        }

        public void ModifyAccount(AccountDto accountDto)
        {
            var account = accountDto.ConvertToModel<AccountDto, Account>();

            var accountResult = Query.FindOne(FilterFactory.Create<Account>(internalAccount => internalAccount.Id == account.Id));

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

        public void Logout()
        {
            AccountContext.Logout(AccountId);

            UnitOfWork.Commit();
        }

        public void Enable()
        {
            var accountResult = Query.FindOne(FilterFactory.Create((Account account) => account.Id == AccountId));

            if (accountResult == null)
            {
                throw new BusinessException("该用户可能已被禁用或被删除，请联系管理员");
            }

            accountResult.Enable();

            Repository.Create<Account>().Update(accountResult);

            UnitOfWork.Commit();

        }

        public void Disable()
        {
            var accountResult = Query.FindOne(FilterFactory.Create((Account account) => account.Id == AccountId));

            if (accountResult == null)
            {
                throw new BusinessException("该用户可能已被禁用或被删除，请联系管理员");
            }

            accountResult.Disable();

            Repository.Create<Account>().Update(accountResult);

            UnitOfWork.Commit();
        }

        public void ModifyAccountFace(String newFace)
        {
            ValidateParameter.Validate(newFace);

            var accountResult = Query.FindOne(FilterFactory.Create((Account account) => account.Id == AccountId));

            accountResult.Config.ModifyAccountFace(newFace);

            Repository.Create<Account>().Update(accountResult);

            UnitOfWork.Commit();
        }

        public void ModifyPassword(String newPassword)
        {
            ValidateParameter.Validate(newPassword);

            var accountResult = Query.FindOne(FilterFactory.Create((Account account) => account.Id == AccountId));

            accountResult.ModifyPassword(PasswordUtil.CreateDbPassword(newPassword));

            Repository.Create<Account>().Update(accountResult);

            UnitOfWork.Commit();
        }

        public void ModifyLockScreenPassword(String newScreenPassword)
        {
            ValidateParameter.Validate(newScreenPassword);

            var accountResult = Query.FindOne(FilterFactory.Create((Account account) => account.Id == AccountId));

            accountResult.ModifyLockScreenPassword(PasswordUtil.CreateDbPassword(newScreenPassword));

            Repository.Create<Account>().Update(accountResult);

            UnitOfWork.Commit();
        }


    }
}
