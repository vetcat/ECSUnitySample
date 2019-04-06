using Signals;
using UniRx;
using Unity.Entities;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Systems
{
    [DisableAutoCreation]
    public class PlayerSpawnSystem : ComponentSystem, IPrioritySystem
    {
        public int Priority { get; }

        private readonly SignalBus _signalBus;
        private readonly PlayerFactorySystem _playerFactorySystem;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        public PlayerSpawnSystem(int priority, SignalBus signalBus, PlayerFactorySystem playerFactorySystem)
        {
            _signalBus = signalBus;
            _playerFactorySystem = playerFactorySystem;
            Priority = priority;
        }

        protected override void OnCreateManager()
        {
            _signalBus.GetStream<SignalUiLayerWantsAddPlayer>()
                .Subscribe(x => {Add(x.Count);})
                .AddTo(_disposables);

            _signalBus.GetStream<SignalUiLayerWantsRemovePlayer>()
                .Subscribe(Remove)
                .AddTo(_disposables);

            _signalBus.GetStream<SignalStartGame>()
                .Subscribe(x => {Add(1);})
                .AddTo(_disposables);
        }

        private void Remove(SignalUiLayerWantsRemovePlayer data)
        {
            if (_playerFactorySystem.SpawnList.Count > 0)
            {
                _playerFactorySystem.Destroy(_playerFactorySystem.SpawnList[0]);
            }
        }

        protected override void OnDestroyManager()
        {
            _disposables?.Dispose();
        }

        protected override void OnUpdate()
        {
        }

        private void Add(int count)
        {
            for (var i = 0; i < count; i++)
            {
                var position = new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));
                var rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
                _playerFactorySystem.Create(position, rotation);
            }
        }
    }
}