using Cysharp.Threading.Tasks;
using Infrastructure.StateMachines.GameStateMachine.States.Interfaces;
using UnityEngine.SceneManagement;

namespace Infrastructure.StateMachines.GameStateMachine.States
{
    public class GameLoopState : IState
    {
        private readonly GameStateMachine _gameStateMachine;

        private const string GameLoopSceneName = "GameLoop";

        public GameLoopState(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
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