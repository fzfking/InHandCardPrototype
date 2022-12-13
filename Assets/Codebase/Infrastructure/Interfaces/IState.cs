namespace Codebase.Infrastructure.Interfaces
{
    public interface IState
    {
        public void Enter();
        public void Exit();
    }
}