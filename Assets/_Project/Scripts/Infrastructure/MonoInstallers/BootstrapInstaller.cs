using _Project.Scripts.Infrastructure.Network;
using Unity.VisualScripting;
using UnityEngine;
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