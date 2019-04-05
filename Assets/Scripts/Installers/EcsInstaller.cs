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
        private PrefabsInstaller.Settings _settings;
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
            //Container.BindInterfacesAndSelfTo<TestSystem>().AsSingle().WithArguments(20).NonLazy();
            //Container.BindInterfacesAndSelfTo<TestSystemJob>().AsSingle().WithArguments(30).NonLazy();

            Container.BindInterfacesAndSelfTo<InputSystem>().AsSingle().WithArguments(30).NonLazy();

            Container.BindInterfacesAndSelfTo<DeltaTimeUpdateSystem>().AsSingle().WithArguments(10).NonLazy();

            Container.BindInterfacesAndSelfTo<EnemyCalculateCountSystem>().AsSingle().WithArguments(20).NonLazy();
            Container.BindInterfacesAndSelfTo<EntityRemoveSystem>().AsSingle().WithArguments(40).NonLazy();
            Container.BindInterfacesAndSelfTo<EnemySpawnSystem>().AsSingle().WithArguments(50).NonLazy();
        }

        private void BindSignals()
        {
            Container.DeclareSignal<SignalUiLayerWantsAddEnemy>();
            Container.DeclareSignal<SignalUiLayerWantsRemoveEnemy>();
            Container.DeclareSignal<SignalEcsLayerEnemyCountUpdate>();
        }

        private void BindFactories()
        {
            Container.BindMemoryPool<EnemyFacade, EnemyFacade.Pool>()
                .WithInitialSize(10)
                .FromComponentInNewPrefab(_settings.enemy)
                .UnderTransformGroup("Pool_Enemy");
        }
    }
}