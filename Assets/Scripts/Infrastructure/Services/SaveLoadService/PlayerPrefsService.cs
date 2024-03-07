using Infrastructure.Data.PlayerData;
using Infrastructure.Services.DataProvider.Interfaces;
using Infrastructure.Services.Json.Interfaces;
using Infrastructure.Services.SaveLoadService.Interfaces;
using UnityEngine;

namespace Infrastructure.Services.SaveLoadService
{
    public class PlayerPrefsService : ISaveLoadService
    {
        private const string ProgressKey = "Progress";

        private readonly IPlayerDataProvider _playerDataProvider;
        private readonly ISerializer _serializer;

        public PlayerPrefsService(IPlayerDataProvider playerDataProvider, ISerializer serializer)
        {
            _playerDataProvider = playerDataProvider;
            _serializer = serializer;
        }

        public void SaveProgress()
        {
            string saveData = _serializer.Serialize(_playerDataProvider.PlayerDataContainer);
            PlayerPrefs.SetString(ProgressKey, saveData);
        }

        public PlayerDataContainer LoadProgress()
        {
            string data = PlayerPrefs.GetString(ProgressKey);
            
            return _serializer.Deserialize<PlayerDataContainer>(data);
        }
    }
}