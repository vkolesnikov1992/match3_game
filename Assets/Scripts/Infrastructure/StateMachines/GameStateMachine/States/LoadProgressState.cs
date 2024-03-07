using Cysharp.Threading.Tasks;
using Infrastructure.Data.ConfigsData.Configs;
using Infrastructure.Data.PlayerData;
using Infrastructure.Data.StaticData;
using Infrastructure.Services.DataProvider.Interfaces;
using Infrastructure.Services.Json.Interfaces;
using Infrastructure.Services.SaveLoadService.Interfaces;
using Infrastructure.StateMachines.GameStateMachine.States.Interfaces;


namespace Infrastructure.StateMachines.GameStateMachine.States
{
    public class LoadProgressState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IDataProvider _dataProvider;
        private readonly ISerializer _serializer;
        private readonly ISaveLoadService _saveLoadService;

        public LoadProgressState(GameStateMachine gameStateMachine, IDataProvider dataProvider, 
            ISerializer serializer, ISaveLoadService saveLoadService)
        {
            _gameStateMachine = gameStateMachine;
            _dataProvider = dataProvider;
            _serializer = serializer;
            _saveLoadService = saveLoadService;
        }
        public void Exit()
        {
            
        }

        public async void Enter()
        {
            await DeserializeConfigs();
            LoadProgressOrInitNew();
            
            _gameStateMachine.Enter<GameLoopState>();
        }

        private async UniTask DeserializeConfigs()
        {
            _dataProvider.ConfigsDataContainer = new ConfigsDataContainer
            {
                LevelSettings = await _serializer.DeserializeConfig<LevelSetting[]>
                    (ConfigsId.Levels_settings, false),
                
                GameSettings = await _serializer.DeserializeConfig<GameSettings>
                    (ConfigsId.Game_settings, true)
            };
        }

        private void LoadProgressOrInitNew()
        {
            _dataProvider.PlayerDataContainer = _saveLoadService.LoadProgress() ?? new PlayerDataContainer();
        }

        private PlayerDataContainer NewPlayerDataContainer() => new PlayerDataContainer();
    }
}