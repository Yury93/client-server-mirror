
using Assets._Project.Scripts.Infrastructure.Services;
using Zenject;

namespace _Project.Scripts.Infrastructure.MonoInstallers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            var nickService = ProjectContext.Instance.Container.Resolve<IMessageService>();
            Container.Bind<IMessageService>().FromInstance(nickService);
        }
    }
}