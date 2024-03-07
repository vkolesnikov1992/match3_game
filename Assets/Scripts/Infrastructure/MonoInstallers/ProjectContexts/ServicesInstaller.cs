using Infrastructure.Factory.GameFactory;
using Infrastructure.Services.DataProvider;
using Infrastructure.Services.InputService;
using Infrastructure.Services.Json;
using Infrastructure.Services.SaveLoadService;
using Zenject;

namespace Infrastructure.MonoInstallers.ProjectContexts
{
    public class ServicesInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            RegisterServices();
        }
        
        private void RegisterServices()
        {
            Container.BindInterfacesTo<GameFactory>().AsSingle();
            Container.BindInterfacesTo<NewtonsoftSerializer>().AsSingle();

            Container.BindInterfacesTo<InputService>().AsSingle();
            Container.BindInterfacesAndSelfTo<DataProvider>().AsSingle();
            Container.BindInterfacesTo<PlayerPrefsService>().AsSingle();
            Container.BindInterfacesTo<ServicesInstaller>().FromInstance(this).AsSingle();
        }
    }
}