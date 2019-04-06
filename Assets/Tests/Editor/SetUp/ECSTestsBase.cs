using Systems;
using Installers;
using NUnit.Framework;
using Providers;
using Unity.Entities;
using UnityEngine;
using Zenject;

namespace Tests.Editor.SetUp
{
    public abstract class EcsTestsBase
    {
        public DiContainer Container;
        public GameSettings GameSettings;
        public PlayerFacade.Pool PlayerPool;
        public PlayerFactorySystem PlayerFactorySystem;
        public TimeProvider TimeProvider;
        public UiSettingsInstaller UiSettings;
        public Canvas Canvas;

        [SetUp]
        public void SetUp()
        {
            Install();
        }

        protected virtual void Install()
        {
            DefaultWorldInitialization.Initialize("TestWorld", false);

            Container = new DiContainer();
            GameSettings = Resources.Load("GameSettings") as GameSettings;
            Container.BindInstances(GameSettings);

            Container.BindInterfacesAndSelfTo<TimeProvider>().AsSingle().NonLazy();
            TimeProvider = Container.Resolve<TimeProvider>();

            Container.BindMemoryPool<PlayerFacade, PlayerFacade.Pool>()
                .WithInitialSize(10)
                .FromComponentInNewPrefab(GameSettings.prefabs.player)
                .UnderTransformGroup("Pool_Players");

            PlayerPool = Container.Resolve<PlayerFacade.Pool>();

            Container.BindInterfacesAndSelfTo<PlayerFactorySystem>().AsSingle().WithArguments(1010).NonLazy();
            PlayerFactorySystem = Container.Resolve<PlayerFactorySystem>();
            World.Active.AddManager(PlayerFactorySystem);

            //UI
            UiSettings = Resources.Load("UiSettingsInstaller") as UiSettingsInstaller;
            Canvas = new GameObject("Canvas").AddComponent<Canvas>();
        }
    }
}