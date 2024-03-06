using Zenject;

namespace Infrastructure.MonoInstallers.SceneContexts
{
    public class GameLoopContext : MonoInstaller
    {
        public override void InstallBindings()
        {
            RegisterServices();
            BindInstaller();
        }
        private void RegisterServices()
        {
            
        }

        private void BindInstaller()
        {
            
        }
    }
}