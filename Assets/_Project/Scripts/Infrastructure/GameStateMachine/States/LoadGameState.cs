using _Project.Scripts.Infrastructure.Network;
using Zenject;

namespace Assets._Project.Scripts.Infrastructure.GameStateMachine.States
{
    public class LoadGameState : IPayLoadedState<string>
    {
        private SceneLoader _sceneLoader;
        private IStateMachine _stateMachine;
        private INetworkService _networkService;
        [Inject]
        public void Construct(SceneLoader sceneLoader,IStateMachine stateMachine,INetworkService networkService)
        {
            this._stateMachine = stateMachine;
            this._sceneLoader = sceneLoader;
            this._networkService = networkService;
        }
        public void Enter(string sceneName)
        {
             
            this._sceneLoader.Load(sceneName, ()=> {
                _networkService.ConnectOrHost();
                _stateMachine.Enter<CreateGameState>(); 
            });
        }

        public void Exit()
        { 
        }
    }
}