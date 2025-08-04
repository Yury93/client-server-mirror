using Assets._Project.Scripts.Infrastructure.Services;
using Assets._Project.Scripts.Infrastructure.Services.GameFactory;
using Assets._Project.Scripts.UI.RegisterWindow;
using Zenject;

namespace Assets._Project.Scripts.Infrastructure.GameStateMachine.States
{
    public class CreateMenuState : IState
    {
        private IGameFactory _gameFactory;
        private IStateMachine _stateMachine;
        private IMessageService _nickService;
        private RegisterWindowController _registerLogic;

        [Inject]
        void Construct(IGameFactory gameFactory, IStateMachine stateMachine, IMessageService nickService)
        {
            _stateMachine = stateMachine;
            _gameFactory = gameFactory;
            _nickService = nickService;
        }
        public async void Enter()
        {
            IRegisterWindow registerWindow = await _gameFactory.CreateRegisterWindow();
            _registerLogic = new RegisterWindowController(registerWindow, _nickService);
            _registerLogic.onRegistered += OnRegistered;
        }

        private void OnRegistered()
        {

            _stateMachine.Enter<LoadGameState, string>("Game");
        }

        public void Exit()
        {
            _registerLogic.onRegistered -= OnRegistered;
            _registerLogic = null;
        }
    }
}