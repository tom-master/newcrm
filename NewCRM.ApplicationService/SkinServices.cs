using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NewCRM.Application.Services.Interface;
using NewCRM.Domain;
using NewCRM.Domain.Services.Interface;
using NewCRM.Infrastructure.CommonTools;

namespace NewCRM.Application.Services
{
    public class SkinServices : BaseServiceContext, ISkinServices
    {
        private readonly ISkinContext _skinContext;

        public SkinServices(ISkinContext skinContext)
        {
            _skinContext = skinContext;
        }

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

        public void ModifySkin(Int32 accountId, String newSkin)
        {
            ValidateParameter.Validate(accountId).Validate(newSkin);
            _skinContext.ModifySkin(accountId, newSkin);
            RemoveOldKeyWhenModify(CacheKey.Config(accountId));
        }

        #region private method

        private String GetLocalImagePath(String fileName, String fullPath)
        {
            ValidateParameter.Validate(fileName).Validate(fullPath);

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
