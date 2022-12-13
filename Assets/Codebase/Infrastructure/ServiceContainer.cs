using System;
using System.Collections.Generic;
using Codebase.Infrastructure.Interfaces;

namespace Codebase.Infrastructure
{
    public class ServiceContainer: IServiceContainer
    {
        private readonly Dictionary<Type, IService> _services;

        public ServiceContainer()
        {
            _services = new Dictionary<Type, IService>();
        }
        public void Register<TService>(TService service) where TService : class, IService
        {
            _services.Add(typeof(TService), service);
        }

        public TService Get<TService>() where TService : class, IService
        {
            return _services[typeof(TService)] as TService;
        }
    }
}