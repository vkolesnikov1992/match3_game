using Infrastructure.StateMachines.GameStateMachine.States.Interfaces;
using Zenject;

namespace Infrastructure.StateMachines.GameStateMachine.States
{
    public class LoadProgressState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly DiContainer _diContainer;

        public LoadProgressState(GameStateMachine gameStateMachine, DiContainer diContainer)
        {
            _gameStateMachine = gameStateMachine;
            _diContainer = diContainer;
        }
        public void Exit()
        {
            
        }

        public void Enter()
        {
            _gameStateMachine.Enter<GameLoopState>();
        }
    }
}