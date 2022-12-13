using System.Collections;
using Codebase.Infrastructure.Interfaces;
using Codebase.Infrastructure.Services.Factories;
using UnityEngine;

namespace Codebase.Infrastructure.States
{
    public class FactoriesInitState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly IServiceContainer _serviceContainer;
        private readonly ICoroutineRunner _coroutineRunner;

        public FactoriesInitState(GameStateMachine stateMachine, IServiceContainer serviceContainer,
            ICoroutineRunner coroutineRunner)
        {
            _stateMachine = stateMachine;
            _serviceContainer = serviceContainer;
            _coroutineRunner = coroutineRunner;
        }

        public void Enter()
        {
            _coroutineRunner.StartCoroutine(InitFactories());
        }

        private IEnumerator InitFactories()
        {
            yield return InitCardIconFactory();
            yield return InitCardFactory();
            Exit();
        }

        private IEnumerator InitCardIconFactory()
        {
            var cardIconFactory = new CardIconFactory(_coroutineRunner);
            _serviceContainer.Register(cardIconFactory);
            yield return new WaitUntil(() => cardIconFactory.IsLoaded);
        }

        private IEnumerator InitCardFactory()
        {
            _serviceContainer.Register(new CardFactory(_serviceContainer.Get<CardIconFactory>()));
            yield return null;
        }

        public void Exit()
        {
            _stateMachine.Enter<SetupLoadState>();
        }
    }
}