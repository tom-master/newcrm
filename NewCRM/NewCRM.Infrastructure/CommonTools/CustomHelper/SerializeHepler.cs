using System.Linq;
using JsonNet.PrivateSettersContractResolvers;
using Newtonsoft.Json;

namespace NewCRM.Infrastructure.CommonTools.CustomHelper
{
    public sealed class SerializeHepler
    {

        private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            ContractResolver = new PrivateSetterContractResolver()
        };

        public static T Serialize<T>(dynamic source)
        {
            var serializeStr = JsonConvert.SerializeObject(source);

            return JsonConvert.DeserializeObject<T>(serializeStr, _settings);
        }


        public static IQueryable<T> Serialize<T>(IQueryable<T> source)
        {
            var serializeStr = JsonConvert.SerializeObject(source);

            return JsonConvert.DeserializeObject<IQueryable<T>>(serializeStr, _settings);
        }
    }
}
