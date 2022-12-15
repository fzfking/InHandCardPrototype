using Codebase.Infrastructure.Interfaces;
using Codebase.Infrastructure.Services.Factories;
using Codebase.Infrastructure.Services.Loaders;
using Codebase.Infrastructure.Services.Storages;
using Codebase.Models;
using Codebase.Presenters;
using UnityEngine;

namespace Codebase.Infrastructure.States
{
    public class GameLoopState: IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly IServiceContainer _serviceContainer;
        private Card[] _cards;
        private SortedCardsContainer _table;

        public GameLoopState(GameStateMachine stateMachine, IServiceContainer serviceContainer)
        {
            _stateMachine = stateMachine;
            _serviceContainer = serviceContainer;
        }
        
        public void Enter()
        {
            InitCards();
            InitContainers();
            InitRandomButton();
            InitPlayField();
            DisableLoadingCurtain();
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

        private void InitPlayField()
        {
            
            int deckSize = Random.Range(6, 9);
            _cards = _serviceContainer.Get<CardStorage>().GetRandomCardDeck(deckSize);
            foreach (var card in _cards)
            {
                _table.AddCard(card);
            }
            _table.Resort();
            _table.MoveAllCardsToSavedValues();
        }

        private void InitContainers()
        {
            _table = _serviceContainer.Get<PlayfieldFactory>().CreateTable(_serviceContainer.Get<CardPresenterFactory>());
            _serviceContainer.Get<PlayfieldFactory>().CreateCardsContainer();
        }

        private void InitRandomButton()
        {
            RandomOffsetButton button = _serviceContainer.Get<PlayfieldFactory>().CreateButton();
            button.Init(_table);
        }

        private void DisableLoadingCurtain()
        {
            _serviceContainer.Get<PlayfieldFactory>().LoadingCurtain.Disable();
        }

        public void Exit()
        {
            _table.Dispose();
            _stateMachine.Enter<ExitState>();
        }
    }
}