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

        public void AddNewPower(Power power)
        {
            PowerRepository.Add(power);
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
