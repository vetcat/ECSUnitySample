using Systems;
using Managers;
using Providers;
using Signals;
using Unity.Entities;
using Zenject;

namespace Installers
{
    public class EcsInstaller : MonoInstaller
    {
        [Zenject.Inject]
        private GameSettings _settings;

        public override void InstallBindings()
        {
            DefaultWorldInitialization.Initialize("SampleWorld", false);

            SignalBusInstaller.Install(Container);

            BindProviders();
            BindSignals();
            BindFactories();
            BindSystems();

            Container.BindInterfacesAndSelfTo<Bootstrap>().AsSingle().NonLazy();
        }

        private void BindProviders()
        {
            Container.Bind<ITimeProvider>().To<TimeProvider>().AsSingle().NonLazy();
            Container.Bind<IInputProvider>().To<InputProvider>().AsSingle().NonLazy();
        }

        private void BindSystems()
        {
            //hi priority
            Container.BindInterfacesAndSelfTo<DeltaTimeUpdateSystem>().AsSingle().WithArguments(10).NonLazy();
            Container.BindInterfacesAndSelfTo<InputSystem>().AsSingle().WithArguments(20).NonLazy();

            //medium priority
            Container.BindInterfacesAndSelfTo<CharacterControllerMovementSystem>().AsSingle().WithArguments(130).NonLazy();
            Container.BindInterfacesAndSelfTo<CharacterControllerRotationSystem>().AsSingle().WithArguments(130).NonLazy();
            Container.BindInterfacesAndSelfTo<PlayerCalculateCountSystem>().AsSingle().WithArguments(140).NonLazy();

            //low priority
            Container.BindInterfacesAndSelfTo<PlayerRemoveSystem>().AsSingle().WithArguments(1050).NonLazy();
            Container.BindInterfacesAndSelfTo<PlayerSpawnSystem>().AsSingle().WithArguments(1060).NonLazy();
        }

        private void BindSignals()
        {
            Container.DeclareSignal<SignalUiLayerWantsAddPlayer>();
            Container.DeclareSignal<SignalUiLayerWantsRemovePlayer>();
            Container.DeclareSignal<SignalEcsLayerPlayerCountUpdate>();
        }

        private void BindFactories()
        {
            Container.BindMemoryPool<PlayerFacade, PlayerFacade.Pool>()
                .WithInitialSize(10)
                .FromComponentInNewPrefab(_settings.prefabs.player)
                .UnderTransformGroup("Pool_Enemy");
        }
    }
}