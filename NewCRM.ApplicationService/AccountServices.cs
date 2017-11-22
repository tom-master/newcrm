using System;
using System.Collections.Generic;
using System.Linq;
using NewCRM.Application.Services.Interface;
using NewCRM.Domain;
using NewCRM.Domain.Entitys.Agent;
using NewCRM.Domain.Services.Interface;
using NewCRM.Domain.ValueObject;
using NewCRM.Dto;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Infrastructure.CommonTools.CustomException;
using NewCRM.Infrastructure.CommonTools.CustomExtension;

namespace NewCRM.Application.Services
{
    public class AccountServices : BaseServiceContext, IAccountServices
    {
        private readonly IAccountContext _accountContext;

        public AccountServices(IAccountContext accountContext)
        {
            _accountContext = accountContext;
        }

        public AccountDto Login(String accountName, String password)
        {
            ValidateParameter.Validate(accountName).Validate(password);

            var account = _accountContext.Validate(accountName, password);
            UnitOfWork.Commit();
            return new AccountDto
            {
                Name = account.Name,
                Id = account.Id
            };
        }

        public ConfigDto GetConfig(Int32 accountId)
        {
            var config = _accountContext.GetConfig(accountId);
            var wallpaper = _accountContext.GetWallpaper(config.WallpaperId);

            return new ConfigDto
            {
                Id = config.Id,
                Skin = config.Skin,
                AccountFace = config.Face,
                AppSize = config.AppSize,
                AppVerticalSpacing = config.AppVerticalSpacing,
                AppHorizontalSpacing = config.AppHorizontalSpacing,
                DefaultDeskNumber = config.DefaultDeskNumber,
                DefaultDeskCount = config.DefaultDeskCount,
                AppXy = config.AppXy.ToString().ToLower(),
                DockPosition = config.DockPosition.ToString().ToLower(),
                WallpaperUrl = wallpaper.Url,
                WallpaperWidth = wallpaper.Width,
                WallpaperHeigth = wallpaper.Height,
                WallpaperSource = wallpaper.Source.ToString().ToLower(),
                WallpaperMode = config.ToString().ToLower()
            };
        }

        public List<AccountDto> GetAccounts(String accountName, String accountType, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
        {
            ValidateParameter.Validate(accountName).Validate(pageIndex).Validate(pageSize);

            var result = _accountContext.GetAccounts(accountName, accountType, pageIndex, pageSize, out totalCount);

            return result.Select(s => new AccountDto
            {
                Id = s.Id,
                IsAdmin = s.IsAdmin,
                Name = s.Name,
                AccountFace = ProfileManager.FileUrl + s.Face,
                IsDisable = s.IsDisable
            }).ToList();
        }

        public AccountDto GetAccount(Int32 accountId)
        {
            var account = _accountContext.GetAccount(accountId);
            var roles = _accountContext.GetRoles(account.Id);
            var powers = _accountContext.GetPowers();

            if(account == null)
            {
                throw new BusinessException("该用户可能已被禁用或被删除，请联系管理员");
            }

            return new AccountDto
            {
                AccountFace = account.Face,
                AddTime = account.AddTime.ToString("yyyy-MM-dd"),
                Id = account.Id,
                IsAdmin = account.IsAdmin,
                IsDisable = account.IsDisable,
                IsOnline = account.IsOnline,
                LastLoginTime = account.LastLoginTime.ToString("yyyy-MM-dd"),
                LastModifyTime = account.LastModifyTime.ToString("yyyy-MM-dd"),
                LockScreenPassword = account.LockScreenPassword,
                Name = account.Name,
                Roles = roles.Select(s => new RoleDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Powers = powers.Where(w => w.RoleId == s.Id).Select(p => new PowerDto
                    {
                        Id = p.AppId
                    }).ToList(),
                    RoleIdentity = s.RoleIdentity
                }).ToList()
            };
        }

        public Boolean CheckAccountNameExist(String accountName)
        {
            ValidateParameter.Validate(accountName);
            return _accountContext.CheckAccountNameExist(accountName);
        }

        public Boolean CheckPassword(Int32 accountId, String oldAccountPassword)
        {
            ValidateParameter.Validate(oldAccountPassword);

            var result = _accountContext.GetOldPassword(accountId);
            return PasswordUtil.ComparePasswords(result, oldAccountPassword);
        }

        public void AddNewAccount(AccountDto accountDto)
        {
            ValidateParameter.Validate(accountDto);

            var account = accountDto.ConvertToModel<AccountDto, Account>();
            var accountType = EnumExtensions.ParseToEnum<AccountType>(account.IsAdmin ? 2 /*管理员*/ : 1 /*用户*/);
            var internalNewAccount = new Account(account.Name, PasswordUtil.CreateDbPassword(account.LoginPassword), account.Roles, accountType);

            _accountContext.AddNewAccount(internalNewAccount);
        }

        public void ModifyAccount(AccountDto accountDto)
        {
            ValidateParameter.Validate(accountDto);

            var account = accountDto.ConvertToModel<AccountDto, Account>();
            _accountContext.ModifyAccount(account);
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
            if(accountResult == null)
            {
                throw new BusinessException("该用户可能已被禁用或被删除，请联系管理员");
            }

            accountResult.Enable();

            _accountRepository.Update(accountResult);
            UnitOfWork.Commit();

        }

        public void Disable(Int32 accountId)
        {
            ValidateParameter.Validate(accountId);

            var accountResult = DatabaseQuery.FindOne(FilterFactory.Create((Account account) => account.Id == accountId));
            if(accountResult.IsAdmin)
            {
                throw new BusinessException($"不能禁用管理员:{accountResult.Name}");
            }

            accountResult.Disable();

            _accountRepository.Update(accountResult);
            UnitOfWork.Commit();
        }

        public void ModifyAccountFace(Int32 accountId, String newFace)
        {
            ValidateParameter.Validate(newFace);

            var accountResult = DatabaseQuery.FindOne(FilterFactory.Create((Account account) => account.Id == accountId));
            accountResult.Config.ModifyAccountFace(newFace);

            _accountRepository.Update(accountResult);
            UnitOfWork.Commit();
        }

        public void ModifyPassword(Int32 accountId, String newPassword)
        {
            ValidateParameter.Validate(newPassword);

            var accountResult = DatabaseQuery.FindOne(FilterFactory.Create((Account account) => account.Id == accountId));
            accountResult.ModifyPassword(PasswordUtil.CreateDbPassword(newPassword));

            _accountRepository.Update(accountResult);
            UnitOfWork.Commit();
        }

        public void ModifyLockScreenPassword(Int32 accountId, String newScreenPassword)
        {
            ValidateParameter.Validate(newScreenPassword);

            var accountResult = DatabaseQuery.FindOne(FilterFactory.Create((Account account) => account.Id == accountId));
            accountResult.ModifyLockScreenPassword(PasswordUtil.CreateDbPassword(newScreenPassword));

            _accountRepository.Update(accountResult);
            UnitOfWork.Commit();
        }

        public void RemoveAccount(Int32 accountId)
        {
            ValidateParameter.Validate(accountId);

            var internalAccount = DatabaseQuery.FindOne(FilterFactory.Create((Account account) => account.Id == accountId));
            if(internalAccount.IsAdmin)
            {
                throw new BusinessException($"不能删除管理员:{internalAccount.Name}");
            }

            internalAccount.Remove();
        }

        public Boolean UnlockScreen(Int32 accountId, String unlockPassword)
        {
            #region 参数验证
            ValidateParameter.Validate(unlockPassword);
            #endregion

            var filter = FilterFactory.Create<Account>(a => a.Id == accountId);
            var result = DatabaseQuery.FindOne(filter);
            if(result == null)
            {
                return false;
            }
            if(PasswordUtil.ComparePasswords(result.LockScreenPassword, unlockPassword))
            {
                return true;
            }
            return false;
        }
    }
}
