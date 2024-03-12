using Infrastructure.MonoBehaviour.UI;
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
            Register();
            RegisterGameSystems();
        }

        private void Register()
        {
            Container.Bind<IInputService>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesTo<CameraService>().AsSingle();
            Container.Bind<GameLoopCanvas>().FromComponentInHierarchy().AsSingle();
        }

        private void RegisterGameSystems()
        {
            Container.BindInterfacesTo<GameplaySystem>().AsSingle();
            Container.BindInterfacesAndSelfTo<Match3System>().AsSingle();
            Container.BindInterfacesAndSelfTo<BallSystem>().AsSingle();
        }
    }
}