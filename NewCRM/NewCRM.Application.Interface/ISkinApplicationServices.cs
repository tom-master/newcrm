﻿using System;
using System.Collections.Generic;

namespace NewCRM.Application.Interface
{
    public interface ISkinApplicationServices
    {
        /// <summary>
        /// 获取全部的皮肤
        /// </summary>
        /// <param name="skinPath"></param>
        /// <returns></returns>
        IDictionary<String, dynamic> GetAllSkin(String skinPath);

        /// <summary>
        /// 修改默认显示的皮肤
        /// </summary>
        /// <param name="newSkin"></param>
        void ModifySkin(String newSkin);
    }
}
