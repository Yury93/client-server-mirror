using Zenject;

namespace _Project.Scripts.Infrastructure.MonoInstallers
{
    public class BootstrapInstaller : MonoInstaller<BootstrapInstaller>
    {

        public override void InstallBindings()
        {
            Container.BindInterfacesTo<Bootstrap>().AsSingle();

        }
    }

}