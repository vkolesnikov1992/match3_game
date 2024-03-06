using Infrastructure.Factory.StateFactory;
using Infrastructure.StateMachines.GameStateMachine;
using Infrastructure.StateMachines.GameStateMachine.States;
using Zenject;

namespace Infrastructure.MonoInstallers.ProjectContexts
{
    public class StateMachineInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            RegisterStates();
            RegisterStateFactory();
            BindInstaller();
        }
        
        private void RegisterStates()
        {
            Container.Bind<BootstrapState>().AsSingle().NonLazy();
            Container.Bind<LoadProgressState>().AsSingle().NonLazy();
            Container.Bind<GameLoopState>().AsSingle().NonLazy();
        }
        
        private void RegisterStateFactory()
        {
            Container.Bind<StateFactory>().AsSingle();
        }

        private void BindInstaller()
        {
            Container.BindInterfacesAndSelfTo<GameStateMachine>().AsSingle();
        }
    }
}