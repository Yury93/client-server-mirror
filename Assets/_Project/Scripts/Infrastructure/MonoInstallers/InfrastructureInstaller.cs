using _Project.Scripts.Infrastructure.Network;
using Assets._Project.Scripts.Infrastructure;
using Assets._Project.Scripts.Infrastructure.GameStateMachine;
using Assets._Project.Scripts.Infrastructure.GameStateMachine.States;
using Assets._Project.Scripts.Infrastructure.Services;
using Assets._Project.Scripts.Infrastructure.Services.GameFactory;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Infrastructure.MonoInstallers
{
    public class InfrastructureInstaller : MonoInstaller<InfrastructureInstaller>, ICoroutineRunner
    {
        public override void InstallBindings()
        {
            BindNetworkManager();
            BindSceneLoader();
            BindGameFactory();
            BindStateMachine();
            BindStates();
            Container.Bind<IMessageService>()
               .To<NickService>()
               .AsSingle()
               .NonLazy();
        }

        private void BindSceneLoader() => Container.Bind<SceneLoader>().AsSingle().WithArguments(this);

        private void BindGameFactory()
        {
            Container.Bind<IAssetProvider>().To<AssetProvider>().AsSingle().NonLazy();
            Container.Bind<IGameFactory>().To<GameFactory>().AsSingle().NonLazy();
        }
        private void BindStateMachine()
        {
            Container.Bind<IStateFactory>().To<StateFactory>().AsSingle();
            Container.Bind<IStateMachine>().To<GameStateMachine>().AsSingle();
        }

        private void BindStates()
        {
            Container.Bind<CreateMenuState>().AsSingle();
            Container.Bind<CreateGameState>().AsSingle();
            Container.Bind<LoadGameState>().AsSingle();
        }
        private void BindNetworkManager()
        {
            var customNetworkManager = GameObject.FindAnyObjectByType<CustomNetworkManager>();
            Container.Bind<INetworkService>().FromInstance(customNetworkManager);
        }
    }
}
