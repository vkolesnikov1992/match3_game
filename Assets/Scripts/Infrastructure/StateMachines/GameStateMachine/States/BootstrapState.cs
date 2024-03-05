using Infrastructure.StateMachines.GameStateMachine.States.Interfaces;

namespace Infrastructure.StateMachines.GameStateMachine.States
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _gameStateMachine;

        public BootstrapState(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }
        public void Exit()
        {
            
        }

        public void Enter()
        {
            _gameStateMachine.Enter<LoadProgressState>();
        }
    }
}