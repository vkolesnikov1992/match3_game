using System;

namespace Infrastructure.Data.PlayerData
{
    [Serializable]
    public class PlayerDataContainer
    {
        public PlayerProgress PlayerProgress;

        public PlayerDataContainer()
        {
            PlayerProgress = new PlayerProgress();
        }
    }
}