using System;
using Infrastructure.Data.ConfigsData.Configs;

namespace Infrastructure.Data.PlayerData
{
    [Serializable]
    public class PlayerProgress
    {
        public int LevelIndex;
        public LevelSetting CurrentLevel;
    }
}