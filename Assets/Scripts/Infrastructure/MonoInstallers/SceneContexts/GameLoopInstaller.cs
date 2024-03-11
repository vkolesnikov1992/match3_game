using Infrastructure.Services.CameraService;
using Infrastructure.Services.InputService.Interfaces;
using Infrastructure.Systems.Core;
using Zenject;

namespace Infrastructure.MonoInstallers.SceneContexts
{
    public class GameLoopInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            RegisterInput();
            RegisterGameSystems();
        }

        private void RegisterInput()
        {
            Container.Bind<IInputService>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesTo<CameraService>().AsSingle();
        }

        private void RegisterGameSystems()
        {
            Container.BindInterfacesTo<GameplaySystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<Match3System>().AsSingle();
            Container.BindInterfacesTo<BallSystem>().AsSingle();
        }
    }
}