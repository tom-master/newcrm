using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace NewCRM.Infrastructure.CommonTools.CustomHelper
{
    public class BingHelper
    {
        private static readonly String _bingUrlPrefix = "http://www.bing.com/";

        public static async Task<String> GetEverydayWallpaperAsync()
        {
            var httpClient = new HttpClient();
            var httpResponseMessage = await httpClient.GetAsync("https://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=1");
            var response = JsonConvert.DeserializeObject<dynamic>(await httpResponseMessage.Content.ReadAsStringAsync());
            var a = ((Newtonsoft.Json.Linq.JObject)response).First.First.First;
            return $@"{_bingUrlPrefix}{((Newtonsoft.Json.Linq.JProperty)a.ToList()[3]).Value}";
        }
    }
}
