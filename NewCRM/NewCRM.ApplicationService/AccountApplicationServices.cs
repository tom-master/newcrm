using System;
using System.Collections.Generic;
using System.Linq;
using NewCRM.Application.Services.Interface;
using NewCRM.Domain;
using NewCRM.Domain.Entitys.Agent;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Factory.DomainSpecification;
using NewCRM.Domain.Services.Interface;
using NewCRM.Domain.ValueObject;
using NewCRM.Dto;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Infrastructure.CommonTools.CustomException;
using NewCRM.Infrastructure.CommonTools.CustomExtension;

namespace NewCRM.Application.Services
{
    public class AccountApplicationServices : BaseServiceContext, IAccountServices
    {
        private readonly IAccountContext _accountContext;

        public AccountApplicationServices(IAccountContext accountContext)
        {
            _accountContext = accountContext;
        }

        public AccountDto Login(String accountName, String password)
        {
            ValidateParameter.Validate(accountName).Validate(password);

            var account = _accountContext.Validate(accountName, password).ConvertToDto<Account, AccountDto>();
            UnitOfWork.Commit();
            return account;

        }

        public ConfigDto GetConfig(Int32 accountId)
        {
            var accountResult = CacheQuery.FindOne(FilterFactory.Create((Account account) => account.Id == accountId));

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
            ValidateParameter.Validate(accountName).Validate(pageIndex).Validate(pageSize);

            var filter = FilterFactory.Create<Account>(account => (accountName + "").Length == 0 || account.Name.Contains(accountName));
            if (!String.IsNullOrEmpty(accountType))
            {
                var isAdmin = (EnumExtensions.ParseToEnum<AccountType>(Int32.Parse(accountType)) == AccountType.Admin);
                filter.And(account => account.IsAdmin == isAdmin);
            }

            return DatabaseQuery.PageBy(filter, pageIndex, pageSize, out totalCount).Select(account => new
            {
                account.Id,
                AccountType = account.IsAdmin ? "2" /*管理员*/ : "1" /*用户*/,
                account.Name,
                account.Config.AccountFace,
                account.IsDisable
            }).ConvertDynamicToDtos<AccountDto>().ToList();

        }

        public AccountDto GetAccount(Int32 accountId = default(Int32))
        {
            var accountResult = CacheQuery.FindOne(FilterFactory.Create((Account account) => account.Id == accountId));
            if (accountResult == null)
            {
                throw new BusinessException("该用户可能已被禁用或被删除，请联系管理员");
            }

            return accountResult.ConvertToDto<Account, AccountDto>();

        }

        public Boolean CheckAccountNameExist(String accountName)
        {
            ValidateParameter.Validate(accountName);
            return !DatabaseQuery.Find(FilterFactory.Create<Account>(account => account.Name == accountName)).Any();
        }

        public Boolean CheckPassword(Int32 accountId, String oldAccountPassword)
        {
            ValidateParameter.Validate(oldAccountPassword);

            var accountResult = DatabaseQuery.FindOne(FilterFactory.Create((Account account) => account.Id == accountId));
            return PasswordUtil.ComparePasswords(accountResult.LoginPassword, oldAccountPassword);
        }

        public void AddNewAccount(AccountDto accountDto)
        {
            ValidateParameter.Validate(accountDto);

            var account = accountDto.ConvertToModel<AccountDto, Account>();
            var accountType = EnumExtensions.ParseToEnum<AccountType>(account.IsAdmin ? 2 /*管理员*/ : 1 /*用户*/);
            var internalNewAccount = new Account(account.Name, PasswordUtil.CreateDbPassword(account.LoginPassword), accountType);
            internalNewAccount.AddRole(account.Roles.Select(role => role.RoleId).ToArray());
            Repository.Create<Account>().Add(internalNewAccount);

            IList<Desk> desks = new List<Desk>();
            for (var i = 1; i <= internalNewAccount.Config.DefaultDeskCount; i++)
            {
                desks.Add(new Desk(i, internalNewAccount.Id));
            }

            Repository.Create<Desk>().Add(desks);
            UnitOfWork.Commit();
        }

        public void ModifyAccount(AccountDto accountDto)
        {
            ValidateParameter.Validate(accountDto);

            var account = accountDto.ConvertToModel<AccountDto, Account>();
            var accountResult = DatabaseQuery.FindOne(FilterFactory.Create<Account>(internalAccount => internalAccount.Id == account.Id));

            if (accountResult == null)
            {
                throw new BusinessException($"用户{account.Name}可能已被禁用或删除");
            }

            if (!String.IsNullOrEmpty(account.LoginPassword))
            {
                var newPassword = PasswordUtil.CreateDbPassword(account.LoginPassword);
                accountResult.ModifyPassword(newPassword);
            }

            if (accountResult.Roles.Any())
            {
                accountResult.Roles.ToList().ForEach(role => { role.Remove(); });
            }

            accountResult.AddRole(account.Roles.Select(role => role.RoleId).ToArray());
            Repository.Create<Account>().Update(accountResult);
            UnitOfWork.Commit();
        }

        public void Logout(Int32 accountId)
        {
            ValidateParameter.Validate(accountId);
            _accountContext.Logout(accountId);

            UnitOfWork.Commit();
        }

        public void Enable(Int32 accountId)
        {
            ValidateParameter.Validate(accountId);

            var accountResult = DatabaseQuery.FindOne(FilterFactory.Create<Account>((account) => account.Id == accountId));
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

            var accountResult = DatabaseQuery.FindOne(FilterFactory.Create((Account account) => account.Id == accountId));
            if (accountResult.IsAdmin)
            {
                throw new BusinessException($"不能禁用管理员:{accountResult.Name}");
            }

            accountResult.Disable();

            Repository.Create<Account>().Update(accountResult);
            UnitOfWork.Commit();
        }

        public void ModifyAccountFace(Int32 accountId, String newFace)
        {
            ValidateParameter.Validate(newFace);

            var accountResult = DatabaseQuery.FindOne(FilterFactory.Create((Account account) => account.Id == accountId));
            accountResult.Config.ModifyAccountFace(newFace);

            Repository.Create<Account>().Update(accountResult);
            UnitOfWork.Commit();
        }

        public void ModifyPassword(Int32 accountId, String newPassword)
        {
            ValidateParameter.Validate(newPassword);

            var accountResult = DatabaseQuery.FindOne(FilterFactory.Create((Account account) => account.Id == accountId));
            accountResult.ModifyPassword(PasswordUtil.CreateDbPassword(newPassword));

            Repository.Create<Account>().Update(accountResult);
            UnitOfWork.Commit();
        }

        public void ModifyLockScreenPassword(Int32 accountId, String newScreenPassword)
        {
            ValidateParameter.Validate(newScreenPassword);

            var accountResult = DatabaseQuery.FindOne(FilterFactory.Create((Account account) => account.Id == accountId));
            accountResult.ModifyLockScreenPassword(PasswordUtil.CreateDbPassword(newScreenPassword));

            Repository.Create<Account>().Update(accountResult);
            UnitOfWork.Commit();
        }

        public void RemoveAccount(Int32 accountId)
        {
            ValidateParameter.Validate(accountId);

            var internalAccount = DatabaseQuery.FindOne(FilterFactory.Create((Account account) => account.Id == accountId));
            if (internalAccount.IsAdmin)
            {
                throw new BusinessException($"不能删除管理员:{internalAccount.Name}"); 
            }

            internalAccount.Remove();
        }
    }
}
