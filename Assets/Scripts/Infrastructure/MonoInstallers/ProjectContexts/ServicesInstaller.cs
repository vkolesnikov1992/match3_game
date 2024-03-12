using Infrastructure.Factory.GameFactory;
using Infrastructure.MonoBehaviour.MonoProvider;
using Infrastructure.Services.DataProvider;
using Infrastructure.Services.DisposableService;
using Infrastructure.Services.Json;
using Infrastructure.Services.MonoProvider.Interfaces;
using Infrastructure.Services.SaveLoadService;
using UnityEngine;
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
            Container.BindInterfacesTo<PlayerPrefsService>().AsSingle();
                
            Container.BindInterfacesAndSelfTo<DataProvider>().AsSingle();
            
            Container.BindInterfacesTo<DisposableService>().AsSingle();

            RegisterMonoProvider();
        }

        private void RegisterMonoProvider()
        {
            GameObject monoProvider = new GameObject("MonoProvider");
            Container.Bind<IMonoProvider>().FromInstance(monoProvider.AddComponent<MonoProvider>()).AsSingle();
            
            DontDestroyOnLoad(monoProvider);
        }
    }
}