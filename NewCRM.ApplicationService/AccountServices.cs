using System;
using System.Collections.Generic;
using System.Linq;
using NewCRM.Application.Services.Interface;
using NewCRM.Domain;
using NewCRM.Domain.Entitys.Agent;
using NewCRM.Domain.Entitys.Security;
using NewCRM.Domain.Services.Interface;
using NewCRM.Domain.ValueObject;
using NewCRM.Dto;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Infrastructure.CommonTools.CustomException;
using NewLib;
using NewLib.Security;

namespace NewCRM.Application.Services
{
    public class AccountServices : BaseServiceContext, IAccountServices
    {
        private readonly IAccountContext _accountContext;

        public AccountServices(IAccountContext accountContext)
        {
            _accountContext = accountContext;
        }

        public AccountDto Login(String accountName, String password, String requestIp)
        {
            ValidateParameter.Validate(accountName).Validate(password);

            var account = _accountContext.Validate(accountName, password, requestIp);
            return new AccountDto
            {
                Name = account.Name,
                Id = account.Id,
                AccountFace = account.AccountFace
            };
        }

        public ConfigDto GetConfig(Int32 accountId)
        {
            var config = GetCache(CacheKey.Config(accountId), () => _accountContext.GetConfig(accountId));
            var wallpaper = GetCache(CacheKey.Wallpaper(accountId), () => _accountContext.GetWallpaper(config.WallpaperId));

            return new ConfigDto
            {
                Id = config.Id,
                Skin = config.Skin,
                AccountFace = config.AccountFace,
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
                WallpaperMode = config.WallpaperMode.ToString().ToLower(),
                IsBing = config.IsBing,
                AccountId = config.AccountId
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
                AccountFace = ProfileManager.FileUrl + s.AccountFace,
                IsDisable = s.IsDisable
            }).ToList();
        }

        public AccountDto GetAccount(Int32 accountId)
        {
            var account = GetCache(CacheKey.Account(accountId), () => _accountContext.GetAccount(accountId));

            if (account == null)
            {
                throw new BusinessException("该用户可能已被禁用或被删除，请联系管理员");
            }

            var roles = GetCache(CacheKey.Roles(account.Id), () => _accountContext.GetRoles(account.Id));
            var powers = GetCache(CacheKey.Powers(), () => _accountContext.GetPowers());

            return new AccountDto
            {
                AccountFace = account.AccountFace,
                AddTime = account.AddTime.ToString("yyyy-MM-dd"),
                Id = account.Id,
                IsAdmin = account.IsAdmin,
                IsDisable = account.IsDisable,
                IsOnline = account.IsOnline,
                LastLoginTime = account.LastLoginTime.ToString("yyyy-MM-dd"),
                LastModifyTime = account.LastModifyTime.ToString("yyyy-MM-dd"),
                LockScreenPassword = account.LockScreenPassword,
                Password = account.LoginPassword,
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
            var accountType = EnumExtensions.ToEnum<AccountType>(account.IsAdmin ? 2 /*管理员*/ : 1 /*用户*/);
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
        }

        public void Enable(Int32 accountId)
        {
            ValidateParameter.Validate(accountId);
            _accountContext.Enable(accountId);
        }

        public void Disable(Int32 accountId)
        {
            ValidateParameter.Validate(accountId);
            _accountContext.Disable(accountId);
        }

        public void ModifyAccountFace(Int32 accountId, String newFace)
        {
            ValidateParameter.Validate(newFace);
            _accountContext.ModifyAccountFace(accountId, newFace);
            RemoveOldKeyWhenModify(CacheKey.Config(accountId));
        }

        public void ModifyPassword(Int32 accountId, String newPassword)
        {
            ValidateParameter.Validate(newPassword);
            _accountContext.ModifyPassword(accountId, PasswordUtil.CreateDbPassword(newPassword));
        }

        public void ModifyLockScreenPassword(Int32 accountId, String newScreenPassword)
        {
            ValidateParameter.Validate(newScreenPassword);
            _accountContext.ModifyLockScreenPassword(accountId, PasswordUtil.CreateDbPassword(newScreenPassword));
            RemoveOldKeyWhenModify(CacheKey.Config(accountId));
        }

        public void RemoveAccount(Int32 accountId)
        {
            ValidateParameter.Validate(accountId);
            _accountContext.RemoveAccount(accountId);
        }

        public Boolean UnlockScreen(Int32 accountId, String unlockPassword)
        {
            ValidateParameter.Validate(unlockPassword);
            return _accountContext.UnlockScreen(accountId, unlockPassword);
        }
    }
}
