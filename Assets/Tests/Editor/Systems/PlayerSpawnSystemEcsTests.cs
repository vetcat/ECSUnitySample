using Systems;
using NUnit.Framework;
using Signals;
using Tests.Editor.SetUp;
using Unity.Entities;
using Zenject;

namespace Tests.Editor.Systems
{
    public class PlayerSpawnSystemEcsTests : EcsTestsBase
    {
        [Test]
        public void PlayerSpawnSystem_SpawnDespawn_FromSignals()
        {
            Container.DeclareSignal<SignalUiLayerWantsAddPlayer>();
            Container.DeclareSignal<SignalUiLayerWantsRemovePlayer>();

            SignalBusInstaller.Install(Container);
            var signalBus = Container.Resolve<SignalBus>();

            Container.BindInterfacesAndSelfTo<PlayerSpawnSystem>().AsSingle().WithArguments(1010).NonLazy();
            var system = Container.Resolve<PlayerSpawnSystem>();

            World.Active.AddManager((ComponentSystemBase) system);

            Assert.AreEqual(0, system.SpawnList.Count);
            Assert.AreEqual(0, PlayerPool.NumActive);

            signalBus.Fire<SignalUiLayerWantsRemovePlayer>();
            Assert.AreEqual(0, system.SpawnList.Count);

            var addPlayersCount = 2;
            signalBus.Fire(new SignalUiLayerWantsAddPlayer(addPlayersCount));
            Assert.AreEqual(addPlayersCount, system.SpawnList.Count);
            Assert.AreEqual(addPlayersCount, PlayerPool.NumActive);


            signalBus.Fire<SignalUiLayerWantsRemovePlayer>();
            Assert.AreEqual(addPlayersCount - 1, system.SpawnList.Count);
        }

        [Test]
        public void PlayerPool_ActivateDeactivate_FromSignals()
        {
            Container.DeclareSignal<SignalUiLayerWantsAddPlayer>();
            Container.DeclareSignal<SignalUiLayerWantsRemovePlayer>();

            SignalBusInstaller.Install(Container);
            var signalBus = Container.Resolve<SignalBus>();

            Container.BindInterfacesAndSelfTo<PlayerSpawnSystem>().AsSingle().WithArguments(1010).NonLazy();
            var system = Container.Resolve<PlayerSpawnSystem>();

            World.Active.AddManager((ComponentSystemBase) system);

            Assert.AreEqual(0, PlayerPool.NumActive);

            var addPlayersCount = 2;
            signalBus.Fire(new SignalUiLayerWantsAddPlayer(addPlayersCount));
            Assert.AreEqual(addPlayersCount, PlayerPool.NumActive);

            signalBus.Fire<SignalUiLayerWantsRemovePlayer>();
            Assert.AreEqual(addPlayersCount -1, PlayerPool.NumActive);
        }

    }
}
