using Infrastructure.Factory.GameFactory;
using Infrastructure.Factory.GameFactory.Interfaces;
using Infrastructure.Services.InputService;
using Infrastructure.Services.InputService.Interfaces;
using Infrastructure.Services.Json;
using Infrastructure.Services.Json.Interfaces;
using Zenject;

namespace Infrastructure.MonoInstallers
{
    public class GlobalContext : MonoInstaller
    {
        public override void InstallBindings()
        {
            RegisterServices();
            BindInstaller();
        }
        
        private void RegisterServices()
        {
            Container.Bind<IGameFactory>().To<GameFactory>().AsSingle();
            Container.Bind<ISerializer>().To<NewtonsoftSerializer>().AsSingle();

            Container.Bind<IInputService>().To<InputService>().AsSingle();
        }

        private void BindInstaller()
        {
            Container.BindInterfacesTo<GlobalContext>().FromInstance(this).AsSingle();
        }
    }
}