using Infrastructure.StateMachines.GameStateMachine.States.Interfaces;
using Zenject;

namespace Infrastructure.Factory.StateFactory
{
    public class StateFactory
    {
        private readonly DiContainer _container;

        public StateFactory(DiContainer container) =>
            _container = container;

        public T CreateState<T>() where T : IExitableState =>
            _container.Resolve<T>();
    }
}