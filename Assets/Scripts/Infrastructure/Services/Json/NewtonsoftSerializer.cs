using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Infrastructure.Services.Json.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Infrastructure.Services.Json
{
    public class NewtonsoftSerializer : ISerializer
    {
        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public async UniTask<T> Deserialize<T>(string id, bool isSingleObject)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(id);
            string data = textAsset.ToString();

            if (isSingleObject)
            {
                JArray jsonArray = JArray.Parse(data);
                data = jsonArray[0].ToObject<JObject>()?.ToString();
            }

            return await UniTask.RunOnThreadPool(() => JsonConvert.DeserializeObject<T>(data));
        }
    }
}