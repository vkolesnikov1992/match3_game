using System;
using System.Collections.Generic;
using Infrastructure.StateMachines.GameStateMachine.States;
using Infrastructure.StateMachines.GameStateMachine.States.Interfaces;
using Zenject;

namespace Infrastructure.StateMachines.GameStateMachine
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;

        public GameStateMachine(DiContainer container)
        {
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(BootstrapState)] = new BootstrapState(this),
                [typeof(LoadProgressState)] = new LoadProgressState(this, container),
                [typeof(GameLoopState)] = new GameLoopState(this, container),
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
            var state = ChangeState<TState>();
            state.Enter();
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();
            TState state = GetState<TState>();
            _activeState = state;
            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState => _states[typeof(TState)] as TState;
    }
}