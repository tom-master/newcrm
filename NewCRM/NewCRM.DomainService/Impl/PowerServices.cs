using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Domain.Entities.DomainModel.Security;
using NewCRM.Infrastructure.CommonTools.CustemException;

namespace NewCRM.Domain.Services.Impl
{
    [Export(typeof(IPowerServices))]
    internal sealed class PowerServices : BaseService, IPowerServices
    {
        public List<dynamic> GetAllPowers()
        {
            var powers = PowerRepository.Entities;
            return powers.OrderByDescending(o => o.AddTime).Select(power => new
            {
                power.Name,
                power.Id,
                power.PowerIdentity
            }).ToList<dynamic>();
        }

        public void AddNewPower(Power power)
        {
            PowerRepository.Add(power);
        }

        public List<Power> GetAllPowers(String powerName, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
        {
            var powers = PowerRepository.Entities;

            if ((powerName + "").Length > 0)
            {
                powers = powers.Where(power => power.Name.Contains(powerName));
            }

            totalCount = powers.Count();

            return powers.OrderByDescending(o => o.AddTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

        }

        public Power GetPower(Int32 powerId)
        {
            var powerResult = PowerRepository.Entities.FirstOrDefault(power => power.Id == powerId);

            if (powerResult == null)
            {
                throw new BusinessException($"该权限可能已被删除，请刷新后再试");
            }
            return powerResult;
        }

        public void ModifyPower(Power power)
        {
            var powerResult = PowerRepository.Entities.FirstOrDefault(p => p.Id == power.Id);
            if (powerResult == null)
            {
                throw new BusinessException("该权限可能已被删除，请刷新后再试");
            }

            powerResult.ModifyPowerIdentity(power.PowerIdentity);

            powerResult.ModifyPowerName(power.Name);

            PowerRepository.Update(powerResult);
        }

        public void RemovePower(Int32 powerId)
        {
            var powerResult = PowerRepository.Entities.FirstOrDefault(power => power.Id == powerId);

            if (powerResult == null)
            {
                throw new BusinessException("该权限可能已被删除，请刷新后再试");
            }

            powerResult.Remove();

            PowerRepository.Update(powerResult);
        }
    }
}
