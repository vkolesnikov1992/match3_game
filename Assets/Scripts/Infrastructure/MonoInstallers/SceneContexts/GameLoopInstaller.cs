using Infrastructure.Services.InputService.Interfaces;
using Zenject;

namespace Infrastructure.MonoInstallers.SceneContexts
{
    public class GameLoopInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            RegisterInput();
        }

        private void RegisterInput()
        {
            Container.Bind<IInputService>().FromComponentInHierarchy().AsSingle();
        }
    }
}