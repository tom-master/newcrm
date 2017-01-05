﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NewCRM.Application.Interface;
using NewCRM.Application.Services.Services;
using NewCRM.Domain.Entitys.Agent;
using NewCRM.Dto.Dto;

namespace NewCRM.Application.Services
{
    [Export(typeof(ISkinApplicationServices))]
    internal class SkinApplicationServices : BaseService, ISkinApplicationServices
    {
        public IDictionary<String, dynamic> GetAllSkin(String skinPath)
        {
            ValidateParameter.Validate(skinPath);

            IDictionary<String, dynamic> dataDictionary = new Dictionary<String, dynamic>();

            Directory.GetFiles(skinPath, "*.css").ToList().ForEach(path =>
            {
                var fileName = Get(path, x => x.LastIndexOf(@"\", StringComparison.OrdinalIgnoreCase) + 1).Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[0];

                dataDictionary.Add(fileName, new
                {
                    cssPath = path.Substring(path.LastIndexOf("script", StringComparison.OrdinalIgnoreCase) - 1).Replace(@"\", "/"),
                    imgPath = GetLocalImagePath(fileName, skinPath)
                });
            });

            return dataDictionary;

        }

        public void ModifySkin(String newSkin)
        {
            ValidateParameter.Validate(newSkin);

            var accountResult = Query.FindOne(FilterFactory.Create((Account account) => account.Id == AccountId));

            accountResult.Config.ModifySkin(newSkin);

            Repository.Create<Account>().Update(accountResult);

            UnitOfWork.Commit();
        }

        #region private method

        private String GetLocalImagePath(String fileName, String fullPath)
        {
            var dic = Directory.GetFiles(fullPath, "preview.png", SearchOption.AllDirectories).ToList();

            foreach (var dicItem in from dicItem in dic let regex = new Regex(fileName) where regex.IsMatch(dicItem) select dicItem)
            {
                return dicItem.Substring(dicItem.LastIndexOf("script", StringComparison.OrdinalIgnoreCase) - 1).Replace(@"\", "/");
            }

            return "";
        }

        private String Get(String path, Func<String, Int32> filterFunc)
        {
            return path.Substring(filterFunc(path));
        }

        #endregion

    }
}
