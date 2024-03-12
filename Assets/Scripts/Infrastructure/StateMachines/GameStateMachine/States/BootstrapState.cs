using Infrastructure.StateMachines.GameStateMachine.States.Interfaces;
using UnityEngine;

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
            SetMaxFrameRate();
            
            _gameStateMachine.Enter<LoadProgressState>();
        }

        private void SetMaxFrameRate()
        {
            Application.targetFrameRate = 60;
        }
    }
}