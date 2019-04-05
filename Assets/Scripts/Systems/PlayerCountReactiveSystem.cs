using System;
using Signals;
using UniRx;
using Unity.Entities;
using Zenject;

//it's just an example of a possible organization of reactive systems
namespace Systems
{
    [DisableAutoCreation]
    public class PlayerCountReactiveSystem : IInitializable, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly PlayerSpawnSystem _spawnSystem;

        public PlayerCountReactiveSystem(SignalBus signalBus, PlayerSpawnSystem spawnSystem)
        {
            _signalBus = signalBus;
            _spawnSystem = spawnSystem;
        }

        public void Initialize()
        {
            _spawnSystem.SpawnList.ObserveCountChanged()
                .Subscribe((count) =>
                {
                    _signalBus.Fire(new SignalEcsLayerPlayerCountUpdate(count));
                })
                .AddTo(_disposables);

            //other logic...
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}