using System.Collections;
using Codebase.Infrastructure.Interfaces;
using Codebase.Infrastructure.Services.Factories;
using Codebase.Infrastructure.Services.Loaders;
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
            yield return InitPrefabsFactory();
            yield return InitCardIconFactory();
            yield return InitCardFactory();
            yield return InitCardPresenterFactory();
            Exit();
        }

        private IEnumerator InitCardPresenterFactory()
        {
            _serviceContainer.Register(new CardPresenterFactory(_serviceContainer.Get<PrefabsLoader>()));
            yield return null;
        }

        private IEnumerator InitPrefabsFactory()
        {
            _serviceContainer.Register(new PlayfieldFactory(_serviceContainer.Get<PrefabsLoader>()));
            yield return null;
        }

        private void ShowLoadingCurtain()
        {
            _serviceContainer.Get<PlayfieldFactory>().LoadingCurtain.Enable();
        }

        private IEnumerator InitCardIconFactory()
        {
            _serviceContainer.Get<PlayfieldFactory>().LoadingCurtain.UpdateStatus("Loading images from network...");
            var cardIconFactory = new CardIconFactory(_coroutineRunner);
            _serviceContainer.Register(cardIconFactory);
            yield return new WaitUntil(() => cardIconFactory.IsLoaded);
        }

        private IEnumerator InitCardFactory()
        {
            _serviceContainer.Get<PlayfieldFactory>().LoadingCurtain.UpdateStatus("Creating cards...");
            _serviceContainer.Register(new CardFactory(_serviceContainer.Get<CardIconFactory>()));
            yield return null;
        }

        public void Exit()
        {
            _stateMachine.Enter<SetupLoadState>();
        }
    }
}