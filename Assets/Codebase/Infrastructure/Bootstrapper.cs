using System;
using Codebase.Infrastructure.Interfaces;
using Codebase.Infrastructure.States;
using UnityEngine;

namespace Codebase.Infrastructure
{
    public class Bootstrapper : MonoBehaviour, ICoroutineRunner
    {
        private IServiceContainer _serviceContainer;
        private GameStateMachine _stateMachine;
        private void Awake()
        {
            _serviceContainer = new ServiceContainer();
            _stateMachine = new GameStateMachine(_serviceContainer, this);
            _stateMachine.Enter<LoadersInitState>();
        }

        private void OnDestroy()
        {
            _stateMachine.Exit();
        }
    }
}
