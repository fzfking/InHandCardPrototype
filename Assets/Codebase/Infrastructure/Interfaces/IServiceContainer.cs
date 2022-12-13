namespace Codebase.Infrastructure.Interfaces
{
    public interface IServiceContainer
    {
        public void Register<TService>(TService service) where TService : class, IService;
        public TService Get<TService>() where TService : class, IService;
    }
}