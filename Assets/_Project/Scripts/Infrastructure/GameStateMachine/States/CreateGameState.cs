using Zenject;

namespace Assets._Project.Scripts.Infrastructure.GameStateMachine.States
{
    public class CreateGameState : IState
    {
        private IStateMachine _stateMachine;
        [Inject]
        void Construct(IStateMachine stateMachine)
        {
            this._stateMachine = stateMachine;
        }
        public void Enter()
        {

        }

        public void Exit()
        {

        }
    }
}