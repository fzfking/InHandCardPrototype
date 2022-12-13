using Codebase.Infrastructure.Interfaces;
using Codebase.Infrastructure.Services.Loaders;

namespace Codebase.Infrastructure.States
{
    public class SetupLoadState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly IServiceContainer _serviceContainer;

        public SetupLoadState(GameStateMachine stateMachine, IServiceContainer serviceContainer)
        {
            _stateMachine = stateMachine;
            _serviceContainer = serviceContainer;
        }

        public void Enter()
        {
            RegisterCardSetupsLoader();
            Exit();
        }

        private void RegisterCardSetupsLoader()
        {
            _serviceContainer.Register(new CardSetupsLoader());
        }

        public void Exit()
        {
            _stateMachine.Enter<GameLoopState>();
        }
    }
}