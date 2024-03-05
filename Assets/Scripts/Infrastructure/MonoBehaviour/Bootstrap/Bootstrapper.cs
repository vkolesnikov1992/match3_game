using Infrastructure.StateMachines.GameStateMachine;
using Infrastructure.StateMachines.GameStateMachine.States;
using Zenject;

namespace Infrastructure.MonoBehaviour.Bootstrap
{
    public class Bootstrapper : UnityEngine.MonoBehaviour
    {
        private DiContainer _diContainer;
        
        [Inject]
        public void Construct(DiContainer diContainer)
        {
            _diContainer = diContainer;
        }
        private void Awake()
        {
            GameStateMachine gameStateMachine = new GameStateMachine(_diContainer);
            gameStateMachine.Enter<BootstrapState>();
        }
    }
}