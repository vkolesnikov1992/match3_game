using Cysharp.Threading.Tasks;
using Infrastructure.Services.DisposableService.Interfaces;
using Infrastructure.StateMachines.GameStateMachine.States.Interfaces;
using UnityEngine.SceneManagement;

namespace Infrastructure.StateMachines.GameStateMachine.States
{
    public class GameLoopState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IDisposableService _disposableService;

        private const string GameLoopSceneName = "GameLoop";

        public GameLoopState(GameStateMachine gameStateMachine, IDisposableService disposableService)
        {
            _gameStateMachine = gameStateMachine;
            _disposableService = disposableService;
        }
        public void Exit()
        {
            _disposableService.DisposeAll();
        }

        public async void Enter()
        {
            await LoadGameLoopScene();
        }
        
        private async UniTask LoadGameLoopScene()
        {
            await SceneManager.LoadSceneAsync(GameLoopSceneName, LoadSceneMode.Single);
        }
    }
}