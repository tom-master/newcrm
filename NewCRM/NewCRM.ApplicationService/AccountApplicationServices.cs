using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Application.Interface;
using NewCRM.Domain;
using NewCRM.Domain.DomainSpecification;
using NewCRM.Domain.Entitys.Agent;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Interface.BoundedContext.Agent;
using NewCRM.Domain.ValueObject;
using NewCRM.Dto;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Infrastructure.CommonTools.CustomException;

namespace NewCRM.Application.Services
{
    [Export(typeof(IAccountApplicationServices))]
    internal class AccountApplicationServices : IAccountApplicationServices
    {
        [Import]
        private BaseServiceContext BaseContext { get; set; }

        private readonly IAccountContext _accountContext;

        [ImportingConstructor]
        public AccountApplicationServices(IAccountContext accountContext)
        {
            _accountContext = accountContext;
        }

        public AccountDto Login(String accountName, String password)
        {
            BaseContext.ValidateParameter.Validate(accountName).Validate(password);

            var account = _accountContext.Validate(accountName, password).ConvertToDto<Account, AccountDto>();

            BaseContext.UnitOfWork.Commit();

            return account;

        }

        public ConfigDto GetConfig()
        {
            var accountResult = BaseContext.Query.FindOne((Account account) => account.Id == BaseContext.GetAccountId());

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
                accountConfig.DefaultDeskCount,
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
            BaseContext.ValidateParameter.Validate(accountName).Validate(pageIndex).Validate(pageSize);

            var specification = BaseContext.FilterFactory.Create<Account>(account => (accountName + "").Length == 0 || account.Name.Contains(accountName));

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

            return BaseContext.Query.PageBy(specification, pageIndex, pageSize, out totalCount).Select(account => new
            {
                account.Id,
                AccountType = account.IsAdmin ? "2" /*管理员*/ : "1" /*用户*/,
                account.Name,
                account.Config.AccountFace
            }).ConvertDynamicToDtos<AccountDto>().ToList();

        }

        public AccountDto GetAccount(Int32 accountId = default(Int32))
        {

            Account accountResult = BaseContext.Query.FindOne((Account account) => account.Id == (accountId == default(Int32) ? BaseContext.GetAccountId() : accountId));

            if (accountResult == null)
            {
                throw new BusinessException("该用户可能已被禁用或被删除，请联系管理员");
            }

            return accountResult.ConvertToDto<Account, AccountDto>();

        }

        public Boolean CheckAccountNameExist(String accountName)
        {
            BaseContext.ValidateParameter.Validate(accountName);

            return !BaseContext.Query.Find(BaseContext.FilterFactory.Create<Account>(account => account.Name == accountName)).Any();

        }

        public Boolean CheckPassword(String oldAccountPassword)
        {
            BaseContext.ValidateParameter.Validate(oldAccountPassword);

            var accountResult = BaseContext.Query.FindOne(BaseContext.FilterFactory.Create((Account account) => account.Id == BaseContext.GetAccountId()));

            return PasswordUtil.ComparePasswords(accountResult.LoginPassword, oldAccountPassword);
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

            BaseContext.Repository.Create<Account>().Add(internalNewAccount);

            IList<Desk> desks = new List<Desk>();

            for (int i = 1; i <= internalNewAccount.Config.DefaultDeskCount; i++)
            {
                desks.Add(new Desk(i, internalNewAccount.Id));
            }

            BaseContext.Repository.Create<Desk>().Add(desks);

            BaseContext.UnitOfWork.Commit();
        }

        public void ModifyAccount(AccountDto accountDto)
        {
            var account = accountDto.ConvertToModel<AccountDto, Account>();

            var accountResult = BaseContext.Query.FindOne(BaseContext.FilterFactory.Create<Account>(internalAccount => internalAccount.Id == account.Id));

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

            BaseContext.Repository.Create<Account>().Update(accountResult);

            BaseContext.UnitOfWork.Commit();

        }

        public void Logout()
        {
            _accountContext.Logout(BaseContext.GetAccountId());

            BaseContext.UnitOfWork.Commit();
        }

        public void Enable()
        {
            var accountResult = BaseContext.Query.FindOne(BaseContext.FilterFactory.Create((Account account) => account.Id == BaseContext.GetAccountId()));

            if (accountResult == null)
            {
                throw new BusinessException("该用户可能已被禁用或被删除，请联系管理员");
            }

            accountResult.Enable();

            BaseContext.Repository.Create<Account>().Update(accountResult);

            BaseContext.UnitOfWork.Commit();

        }

        public void Disable()
        {
            var accountResult = BaseContext.Query.FindOne(BaseContext.FilterFactory.Create((Account account) => account.Id == BaseContext.GetAccountId()));

            if (accountResult == null)
            {
                throw new BusinessException("该用户可能已被禁用或被删除，请联系管理员");
            }

            accountResult.Disable();

            BaseContext.Repository.Create<Account>().Update(accountResult);

            BaseContext.UnitOfWork.Commit();
        }

        public void ModifyAccountFace(String newFace)
        {
            BaseContext.ValidateParameter.Validate(newFace);

            var accountResult = BaseContext.Query.FindOne(BaseContext.FilterFactory.Create((Account account) => account.Id == BaseContext.GetAccountId()));

            accountResult.Config.ModifyAccountFace(newFace);

            BaseContext.Repository.Create<Account>().Update(accountResult);

            BaseContext.UnitOfWork.Commit();
        }

        public void ModifyPassword(String newPassword)
        {
            BaseContext.ValidateParameter.Validate(newPassword);

            var accountResult = BaseContext.Query.FindOne(BaseContext.FilterFactory.Create((Account account) => account.Id == BaseContext.GetAccountId()));

            accountResult.ModifyPassword(PasswordUtil.CreateDbPassword(newPassword));

            BaseContext.Repository.Create<Account>().Update(accountResult);

            BaseContext.UnitOfWork.Commit();
        }

        public void ModifyLockScreenPassword(String newScreenPassword)
        {
            BaseContext.ValidateParameter.Validate(newScreenPassword);

            var accountResult = BaseContext.Query.FindOne(BaseContext.FilterFactory.Create((Account account) => account.Id == BaseContext.GetAccountId()));

            accountResult.ModifyLockScreenPassword(PasswordUtil.CreateDbPassword(newScreenPassword));

            BaseContext.Repository.Create<Account>().Update(accountResult);

            BaseContext.UnitOfWork.Commit();
        }


    }
}
