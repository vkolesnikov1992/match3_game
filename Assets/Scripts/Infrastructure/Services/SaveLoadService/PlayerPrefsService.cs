using Infrastructure.Data.PlayerData;
using Infrastructure.Services.DataProvider.Interfaces;
using Infrastructure.Services.Json.Interfaces;
using Infrastructure.Services.MonoProvider.Interfaces;
using Infrastructure.Services.SaveLoadService.Interfaces;
using UnityEngine;
using Zenject;

namespace Infrastructure.Services.SaveLoadService
{
    public class PlayerPrefsService : ISaveLoadService, IInitializable
    {
        private const string ProgressKey = "Progress";

        private readonly IPlayerDataProvider _playerDataProvider;
        private readonly ISerializer _serializer;
        private readonly IMonoProvider _monoProvider;

        public PlayerPrefsService(IPlayerDataProvider playerDataProvider, ISerializer serializer, IMonoProvider monoProvider)
        {
            _playerDataProvider = playerDataProvider;
            _serializer = serializer;
            _monoProvider = monoProvider;
        }
        
        public void Initialize()
        {
            _monoProvider.OnApplicationPauseEvent += OnSaveProgress;
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

        private void OnSaveProgress(bool isPause)
        {
            if (isPause)
            {
                SaveProgress();
            }
        }
    }
}