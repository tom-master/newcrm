using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Application.Services.IApplicationService;
using NewCRM.Domain.Entities.DomainModel.Account;
using NewCRM.Domain.Entities.ValueObject;
using NewCRM.Dto;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools.CustemException;
using NewCRM.QueryServices.DomainSpecification;

namespace NewCRM.Application.Services
{
    [Export(typeof(IAccountApplicationServices))]
    internal class AccountApplicationServices : BaseApplicationServices, IAccountApplicationServices
    {
        public AccountDto Login(String accountName, String password)
        {
            ValidateParameter.Validate(accountName).Validate(password);
            return AccountContext.Validate(accountName, password).ConvertToDto<Account, AccountDto>();
        }

        public ConfigDto GetConfig(Int32 accountId)
        {
            ValidateParameter.Validate(accountId);

            ISpecification<Account> accountSpecification = new Specification<Account>(account => account.Id == accountId);

            var accountConfig = Query.CreateQuery<Account>().Find(accountSpecification).FirstOrDefault()?.Config;

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

            ISpecification<Account> specification = new Specification<Account>(account => (accountName + "").Length == 0 || account.Name.Contains(accountName));



            AccountType internalAccountType;
            if ((accountType + "").Length > 0)
            {
                var enumConst = Enum.GetName(typeof(AccountType), accountType);

                if (Enum.TryParse(enumConst, true, out internalAccountType))
                {
                    specification.And(new Specification<Account>(account => account.IsAdmin == (internalAccountType == AccountType.Admin)));
                }
                else
                {
                    throw new BusinessException($"用户类型{accountType}不是有效的类型");
                }
            }

            return Query.CreateQuery<Account>().PageBy(specification, pageIndex, pageSize, out totalCount).Select(account => new
            {
                account.Id,
                AccountType = account.IsAdmin ? "2" /*管理员*/ : "1" /*用户*/,
                account.Name
            }).ConvertDynamicToDtos<AccountDto>().ToList();
        }

        public AccountDto GetAccount(Int32 accountId)
        {
            ValidateParameter.Validate(accountId);

            var accountResult = Query.CreateQuery<Account>().Find(new Specification<Account>(account => account.Id == accountId)).FirstOrDefault();
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
                Roles = accountResult.Roles.Select(s => new
                {
                    Id = s.RoleId
                })
            });
        }

        public void AddNewAccount(AccountDto accountDto)
        {
            ValidateParameter.Validate(accountDto);

            SecurityContext.AddNewAccount(accountDto.ConvertToModel<AccountDto, Account>());
        }

        public Boolean CheckAccountNameExist(String accountName)
        {
            ValidateParameter.Validate(accountName);

            return Query.CreateQuery<Account>().Find(new Specification<Account>(account => account.Name == accountName)).Any();
        }

        public void ModifyAccount(AccountDto account)
        {
            ValidateParameter.Validate(account);
            SecurityContext.ModifyAccount(account.ConvertToModel<AccountDto, Account>());
        }

        public void Logout(Int32 accountId)
        {
            ValidateParameter.Validate(accountId);
            AccountContext.Logout(accountId);
        }

        public void Enable(Int32 accountId)
        {
            ValidateParameter.Validate(accountId);

            Query.CreateQuery<Account>().Find(new Specification<Account>(account => account.Id == accountId)).FirstOrDefault()?.Enable();
        }

        public void Disable(Int32 accountId)
        {
            ValidateParameter.Validate(accountId);

            Query.CreateQuery<Account>().Find(new Specification<Account>(account => account.Id == accountId)).FirstOrDefault()?.Disable();
        }
    }
}
