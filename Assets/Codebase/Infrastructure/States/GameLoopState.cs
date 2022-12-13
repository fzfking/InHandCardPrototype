using Codebase.Infrastructure.Interfaces;
using Codebase.Infrastructure.Services.Factories;
using Codebase.Infrastructure.Services.Loaders;
using Codebase.Infrastructure.Services.Storages;

namespace Codebase.Infrastructure.States
{
    public class GameLoopState: IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly IServiceContainer _serviceContainer;

        public GameLoopState(GameStateMachine stateMachine, IServiceContainer serviceContainer)
        {
            _stateMachine = stateMachine;
            _serviceContainer = serviceContainer;
        }
        
        public void Enter()
        {
            InitCards();
            Exit();
        }

        private void InitCards()
        {
            var cardStorage = new CardStorage();
            var cardFactory = _serviceContainer.Get<CardFactory>();
            foreach (var cardSetup in _serviceContainer.Get<CardSetupsLoader>().GetSetups())
            {
                cardStorage.AddCard(cardFactory.Create(cardSetup));
            }
            _serviceContainer.Register(cardStorage);
        }

        public void Exit()
        {
            
        }
    }
}