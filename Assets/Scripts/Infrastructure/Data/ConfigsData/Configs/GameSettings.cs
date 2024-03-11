using System;

namespace Infrastructure.Data.ConfigsData.Configs
{
    [Serializable]
    public struct GameSettings
    {
        public int CubeAnimationSpeed;
        public int DropSpeed;
        public int SwapSpeed;
        public int MaxBallsCountInGame;
    }
}