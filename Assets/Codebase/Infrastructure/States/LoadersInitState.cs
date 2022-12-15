using Codebase.Infrastructure.Interfaces;
using Codebase.Infrastructure.Services.Loaders;

namespace Codebase.Infrastructure.States
{
    public class LoadersInitState:IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly IServiceContainer _serviceContainer;

        public LoadersInitState(GameStateMachine stateMachine, IServiceContainer serviceContainer)
        {
            _stateMachine = stateMachine;
            _serviceContainer = serviceContainer;
        }
        public void Enter()
        {
            RegisterPrefabsLoader();
            Exit();
        }
        
        private void RegisterPrefabsLoader()
        {
            _serviceContainer.Register(new PrefabsLoader());
        }

        public void Exit()
        {
            _stateMachine.Enter<FactoriesInitState>();
        }
    }
}