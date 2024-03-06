using Infrastructure.StateMachines.GameStateMachine.States.Interfaces;

namespace Infrastructure.StateMachines.GameStateMachine.States
{
    public class LoadProgressState : IState
    {
        private readonly GameStateMachine _gameStateMachine;

        public LoadProgressState(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
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