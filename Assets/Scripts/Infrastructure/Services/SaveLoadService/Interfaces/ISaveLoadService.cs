using Infrastructure.Data.PlayerData;

namespace Infrastructure.Services.SaveLoadService.Interfaces
{
    public interface ISaveLoadService
    {
        void SaveProgress();
        PlayerDataContainer LoadProgress();
    }
}