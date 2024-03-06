using Cysharp.Threading.Tasks;
using Infrastructure.StateMachines.GameStateMachine.States.Interfaces;
using UnityEngine.SceneManagement;
using Zenject;

namespace Infrastructure.StateMachines.GameStateMachine.States
{
    public class GameLoopState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly DiContainer _diContainer;

        private const string GameLoopSceneName = "GameLoop";

        public GameLoopState(GameStateMachine gameStateMachine, DiContainer diContainer)
        {
            _gameStateMachine = gameStateMachine;
            _diContainer = diContainer;
        }
        public void Exit()
        {
            
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