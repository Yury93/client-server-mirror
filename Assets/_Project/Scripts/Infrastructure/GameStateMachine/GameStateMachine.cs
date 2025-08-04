using Zenject;

namespace Assets._Project.Scripts.Infrastructure.GameStateMachine
{
    public interface IState : IExitableState
    {
        void Enter();
    }

    public interface IPayLoadedState<TPayLoad> : IExitableState
    {
        void Enter(TPayLoad payLoad);
    }

    public interface IExitableState
    {
        void Exit();
    }

    public interface IStateMachine
    {
        void Enter<TState>() where TState : class, IState;
        void Enter<TState, TPayLoad>(TPayLoad payLoad) where TState : class, IPayLoadedState<TPayLoad>;
    }

    public class GameStateMachine : IStateMachine
    {
        private readonly IStateFactory _stateFactory;
        private IExitableState _currentState;

        public GameStateMachine(IStateFactory stateFactory) => _stateFactory = stateFactory; 

        public void Enter<TState>() where TState : class, IState
        {
            ExitCurrentState();
            var newState = _stateFactory.Create<TState>();
            _currentState = newState;
            newState.Enter();
        }

        public void Enter<TState, TPayLoad>(TPayLoad payLoad) where TState : class, IPayLoadedState<TPayLoad>
        {
            ExitCurrentState();
            var newState = _stateFactory.Create<TState>();
            _currentState = newState;
            newState.Enter(payLoad);
        }

        private void ExitCurrentState() =>  _currentState?.Exit(); 
    }
}