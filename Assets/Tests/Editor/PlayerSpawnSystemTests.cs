using Systems;
using Installers;
using NUnit.Framework;
using Signals;
using Unity.Entities;
using UnityEngine;
using Zenject;

namespace Tests.Editor
{
    public class PlayerSpawnSystemTests
    {
        [Test]
        public void Test()
        {
            DefaultWorldInitialization.Initialize("TestWorld", false);

            var container = new DiContainer();
            SignalBusInstaller.Install(container);

            var settings = Resources.Load("GameSettings") as GameSettings;
            container.BindInstances(settings);

            container.DeclareSignal<SignalUiLayerWantsAddPlayer>();
            container.DeclareSignal<SignalUiLayerWantsRemovePlayer>();

            container.BindMemoryPool<PlayerFacade, PlayerFacade.Pool>()
                .WithInitialSize(10)
                .FromComponentInNewPrefab(settings.prefabs.player)
                .UnderTransformGroup("Pool_Players");

            container.BindInterfacesAndSelfTo<PlayerSpawnSystem>().AsSingle().WithArguments(1010).NonLazy();
            var system = container.Resolve<PlayerSpawnSystem>();

            World.Active.AddManager((ComponentSystemBase) system);

            system.Update();
        }
    }
}
