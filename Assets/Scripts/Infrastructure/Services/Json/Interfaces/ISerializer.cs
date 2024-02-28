namespace Infrastructure.Services.Json.Interfaces
{
    public interface ISerializer
    {
        void Serialize();
        
        void Deserialize();
    }
}