using System;
using System.Collections.Generic;
using Codebase.Infrastructure.Interfaces;
using Codebase.Infrastructure.States;

namespace Codebase.Infrastructure
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IState> _states;
        private IState _currentState;

        public GameStateMachine(IServiceContainer serviceContainer, ICoroutineRunner coroutineRunner)
        {
            _states = new Dictionary<Type, IState>
            {
                [typeof(FactoriesInitState)] = new FactoriesInitState(this, serviceContainer, coroutineRunner),
                [typeof(SetupLoadState)] = new SetupLoadState(this, serviceContainer),
                [typeof(GameLoopState)] = new GameLoopState(this, serviceContainer),
                [typeof(LoadersInitState)] = new LoadersInitState(this, serviceContainer),
                [typeof(ExitState)] = new ExitState()
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
            _currentState = _states[typeof(TState)];
            _currentState.Enter();
        }

        public void Exit()
        {
            if (_currentState == null)
            {
                throw new Exception("State was null when game stopped.");
            }
            _currentState.Exit();
        }
    }
}