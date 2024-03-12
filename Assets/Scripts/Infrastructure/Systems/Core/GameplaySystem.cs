using System;
using Infrastructure.MonoBehaviour.UI;
using Infrastructure.Services.DataProvider.Interfaces;
using Infrastructure.Services.DisposableService.Interfaces;
using Infrastructure.Services.SaveLoadService.Interfaces;
using Zenject;

namespace Infrastructure.Systems.Core
{
    public class GameplaySystem : IInitializable, IDisposable
    {
        private readonly Match3System _match3System;
        private readonly BallSystem _ballSystem;
        private readonly IDataProvider _dataProvider;
        private readonly ISaveLoadService _saveLoadService;
        private readonly GameLoopCanvas _gameLoopCanvas;
        private readonly IDisposableService _disposableService;

        private int _cubeCountTarget;

        public GameplaySystem(Match3System match3System, BallSystem ballSystem, IDataProvider dataProvider, 
            ISaveLoadService saveLoadService, GameLoopCanvas gameLoopCanvas, IDisposableService disposableService)
        {
            _match3System = match3System;
            _ballSystem = ballSystem;
            _dataProvider = dataProvider;
            _saveLoadService = saveLoadService;
            _gameLoopCanvas = gameLoopCanvas;
            _disposableService = disposableService;
        }

        public void Initialize()
        {
            _disposableService.Track(this);
            
            CreateLevel(GetCurrentLevel());
            
            _match3System.DestroyedCubeCount += CheckLevel;
            _gameLoopCanvas.OnReplayClicked += LevelReload;
            _gameLoopCanvas.OnNextLevelClicked += NextLevel;
        }
        
        public void Dispose()
        {
            _match3System.DestroyedCubeCount -= CheckLevel;
            _gameLoopCanvas.OnReplayClicked -= LevelReload;
            _gameLoopCanvas.OnNextLevelClicked -= NextLevel;
        }

        private void CreateLevel(int[,] level)
        {
            _match3System.CreateLevel(level);
            _cubeCountTarget = GetLevelTargetCount(level);
        }

        private void CheckLevel(int destroyedCubeCount)
        {
            if (destroyedCubeCount == _cubeCountTarget)
            {
                int[,] level = GetNextLevel();
                
                CreateLevel(level);
                
                _saveLoadService.SaveProgress();
            }
        }

        private int GetLevelTargetCount(int[,] level)
        {
            int count = 0;
            for (int i = 0; i < level.GetLength(0); i++)
            {
                for (int j = 0; j < level.GetLength(1); j++)
                {
                    if (level[i, j] > 0)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        private int[,] GetCurrentLevel()
        {
            string levelData = _dataProvider.PlayerDataContainer.PlayerProgress.CurrentLevel.Level;
            if (!string.IsNullOrEmpty(levelData) && GetLevelTargetCount(_dataProvider.PlayerDataContainer.PlayerProgress.CurrentLevel.GetLevelsArray()) > 0)
            {
                return _dataProvider.PlayerDataContainer.PlayerProgress.CurrentLevel.GetLevelsArray();
            }

            int levelIndex = _dataProvider.PlayerDataContainer.PlayerProgress.LevelIndex;
            return GetLevelByIndex(levelIndex);
        }

        private int[,] GetNextLevel()
        {
            int levelIndex = _dataProvider.PlayerDataContainer.PlayerProgress.LevelIndex;
            int levelsCount = _dataProvider.ConfigsDataContainer.LevelSettings.Length;

            int newLevelIndex = (levelIndex + 1) % levelsCount;

            _dataProvider.PlayerDataContainer.PlayerProgress.LevelIndex = newLevelIndex;

            int[,] level = _dataProvider.ConfigsDataContainer.LevelSettings[newLevelIndex].GetLevelsArray();

            return level;
        }
        
        private int[,] GetLevelByIndex(int levelIndex)
        {
            return _dataProvider.ConfigsDataContainer.LevelSettings[levelIndex].GetLevelsArray();
        }

        private void LevelReload()
        {
            _match3System.ClearLevel();
            
            int levelIndex = _dataProvider.PlayerDataContainer.PlayerProgress.LevelIndex;
            CreateLevel(GetLevelByIndex(levelIndex));
            
            _ballSystem.RestartBallMovement();
        }

        private void NextLevel()
        {
            _match3System.ClearLevel();
            CreateLevel(GetNextLevel());
            _ballSystem.RestartBallMovement();
        }
    }
}