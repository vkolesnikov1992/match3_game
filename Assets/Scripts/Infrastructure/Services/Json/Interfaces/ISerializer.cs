using Cysharp.Threading.Tasks;

namespace Infrastructure.Services.Json.Interfaces
{
    public interface ISerializer
    {
        string Serialize(object obj);
        
        UniTask<T> Deserialize<T>(string id, bool isSingleObject);
    }
}