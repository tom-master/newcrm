using System;
using System.Collections.Generic;
using System.Linq;
using NewCRM.Application.Services.Interface;
using NewCRM.Domain;
using NewCRM.Domain.Entitys.Agent;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Factory.DomainSpecification;
using NewCRM.Domain.Repositories.IRepository.Agent;
using NewCRM.Domain.Repositories.IRepository.System;
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
        private readonly IAccountRepository _accountRepository;
        private readonly IDeskRepository _deskRepository;

        public AccountServices(IAccountContext accountContext, IAccountRepository accountRepository, IDeskRepository deskRepository)
        {
            _accountContext = accountContext;
            _accountRepository = accountRepository;
            _deskRepository = deskRepository;
        }

        public AccountDto Login(String accountName, String password)
        {
            ValidateParameter.Validate(accountName).Validate(password);

            var account = _accountContext.Validate(accountName, password);
            UnitOfWork.Commit();
            return new AccountDto
            {
                AccountFace = account.Config.AccountFace,
                Name = account.Name,
                Id = account.Id
            };
        }

        public ConfigDto GetConfig(Int32 accountId)
        {
            var result = CacheQuery.FindOne(FilterFactory.Create((Account account) => account.Id == accountId), a => a.Config);

            return new ConfigDto
            {
                Id = result.Id,
                Skin = result.Skin,
                AccountFace = result.AccountFace,
                AppSize = result.AppSize,
                AppVerticalSpacing = result.AppVerticalSpacing,
                AppHorizontalSpacing = result.AppHorizontalSpacing,
                DefaultDeskNumber = result.DefaultDeskNumber,
                DefaultDeskCount = result.DefaultDeskCount,
                AppXy = result.AppXy.ToString().ToLower(),
                DockPosition = result.DockPosition.ToString().ToLower(),
                WallpaperUrl = result.Wallpaper.Url,
                WallpaperWidth = result.Wallpaper.Width,
                WallpaperHeigth = result.Wallpaper.Height,
                WallpaperSource = result.Wallpaper.Source.ToString().ToLower(),
                WallpaperMode = result.WallpaperMode.ToString().ToLower()
            };
        }

        public List<AccountDto> GetAccounts(String accountName, String accountType, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
        {
            ValidateParameter.Validate(accountName).Validate(pageIndex).Validate(pageSize);

            var filter = FilterFactory.Create<Account>(account => String.IsNullOrEmpty(accountName) || account.Name.Contains(accountName));
            if(!String.IsNullOrEmpty(accountType))
            {
                var isAdmin = (EnumExtensions.ParseToEnum<AccountType>(Int32.Parse(accountType)) == AccountType.Admin);
                filter.And(account => account.IsAdmin == isAdmin);
            }

            return DatabaseQuery.PageBy(filter, pageIndex, pageSize, out totalCount, a => new AccountDto
            {
                Id = a.Id,
                IsAdmin = a.IsAdmin,
                Name = a.Name,
                AccountFace = ProfileManager.FileUrl + a.Config.AccountFace,
                IsDisable = a.IsDisable
            }).ToList();
        }

        public AccountDto GetAccount(Int32 accountId = default(Int32))
        {
            var result = CacheQuery.FindOne(FilterFactory.Create((Account account) => account.Id == accountId), a => new AccountDto
            {
                AccountFace = a.Config.AccountFace,
                AddTime = a.AddTime.ToString("yyyy-MM-dd"),
                Id = a.Id,
                IsAdmin = a.IsAdmin,
                IsDisable = a.IsDisable,
                IsOnline = a.IsOnline,
                LastLoginTime = a.LastLoginTime.ToString("yyyy-MM-dd"),
                LastModifyTime = a.LastModifyTime.ToString("yyyy-MM-dd"),
                LockScreenPassword = a.LockScreenPassword,
                Name = a.Name,
                Roles = a.Roles.Select(s => new RoleDto
                {
                    Id = s.RoleId,
                    Name = s.Role.Name,
                    Powers = s.Role.Powers.Select(p => new PowerDto
                    {
                        Id = p.AppId
                    }).ToList(),
                    RoleIdentity = s.Role.RoleIdentity
                }).ToList(),
            });
            if(result == null)
            {
                throw new BusinessException("该用户可能已被禁用或被删除，请联系管理员");
            }

            return result;
        }

        public Boolean CheckAccountNameExist(String accountName)
        {
            ValidateParameter.Validate(accountName);
            return !DatabaseQuery.Find(FilterFactory.Create<Account>(account => account.Name == accountName), a => a.Name).Any();
        }

        public Boolean CheckPassword(Int32 accountId, String oldAccountPassword)
        {
            ValidateParameter.Validate(oldAccountPassword);

            var result = DatabaseQuery.FindOne(FilterFactory.Create((Account account) => account.Id == accountId), a => a.LoginPassword);
            return PasswordUtil.ComparePasswords(result, oldAccountPassword);
        }

        public void AddNewAccount(AccountDto accountDto)
        {
            ValidateParameter.Validate(accountDto);

            var account = accountDto.ConvertToModel<AccountDto, Account>();
            var accountType = EnumExtensions.ParseToEnum<AccountType>(account.IsAdmin ? 2 /*管理员*/ : 1 /*用户*/);
            var internalNewAccount = new Account(account.Name, PasswordUtil.CreateDbPassword(account.LoginPassword), accountType);

            internalNewAccount.AddRole(account.Roles.Select(role => role.RoleId).ToArray());
            _accountRepository.Add(internalNewAccount);

            var desks = new List<Desk>();
            for(var i = 1 ; i <= internalNewAccount.Config.DefaultDeskCount ; i++)
            {
                desks.Add(new Desk(i, internalNewAccount.Id));
            }

            _deskRepository.Add(desks);
            UnitOfWork.Commit();
        }

        public void ModifyAccount(AccountDto accountDto)
        {
            ValidateParameter.Validate(accountDto);

            var account = accountDto.ConvertToModel<AccountDto, Account>();
            var accountResult = DatabaseQuery.FindOne(FilterFactory.Create<Account>(internalAccount => internalAccount.Id == account.Id));

            if(accountResult == null)
            {
                throw new BusinessException($"用户{account.Name}可能已被禁用或删除");
            }

            if(!String.IsNullOrEmpty(account.LoginPassword))
            {
                var newPassword = PasswordUtil.CreateDbPassword(account.LoginPassword);
                accountResult.ModifyPassword(newPassword);
            }

            if(accountResult.Roles.Any())
            {
                accountResult.Roles.ToList().ForEach(role => { role.Remove(); });
            }

            accountResult.AddRole(account.Roles.Select(role => role.RoleId).ToArray());
            _accountRepository.Update(accountResult);
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
