using Cysharp.Threading.Tasks;

namespace Infrastructure.Services.Json.Interfaces
{
    public interface ISerializer
    {
        string Serialize(object obj);

        T Deserialize<T>(string json);
        
        UniTask<T> DeserializeConfig<T>(string id, bool isSingleObject);
    }
}