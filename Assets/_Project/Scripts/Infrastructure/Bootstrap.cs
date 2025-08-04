using Assets._Project.Scripts.Infrastructure.GameStateMachine;
using Assets._Project.Scripts.Infrastructure.GameStateMachine.States;
using Zenject;

public class Bootstrap : IInitializable
{
    private readonly IStateMachine _stateMachine;
    public Bootstrap(IStateMachine stateMachine)
    {
        _stateMachine = stateMachine;
    }
    public void Initialize()
    {
        _stateMachine.Enter<CreateMenuState>();
    }
}
