namespace Infrastructure.StateMachines.GameStateMachine.States.Interfaces
{
    public interface IState : IExitableState
    {
        void Enter();
    }
}