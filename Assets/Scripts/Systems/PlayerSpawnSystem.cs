using Components;
using Installers;
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
        public readonly ReactiveCollection<PlayerFacade> SpawnList = new ReactiveCollection<PlayerFacade>();

        private readonly SignalBus _signalBus;
        private readonly PlayerFacade.Pool _pool;
        private readonly GameSettings _settings;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        public PlayerSpawnSystem(int priority, SignalBus signalBus, PlayerFacade.Pool pool, GameSettings settings)
        {
            _signalBus = signalBus;
            _pool = pool;
            _settings = settings;
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
        }

        private void Remove(SignalUiLayerWantsRemovePlayer data)
        {
            if (SpawnList.Count == 0)
                return;

            var deSpawn = SpawnList[0];
            _pool.Despawn(deSpawn);
            SpawnList.Remove(deSpawn);
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
                var spawnPosition = new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));
                Spawn(spawnPosition, Quaternion.Euler(0f, Random.Range(0f, 360f), 0f));
            }
        }

        private void Spawn(Vector3 position, Quaternion rotation)
        {
            var spawn = _pool.Spawn(position, rotation);
            var entity = spawn.gameObject.GetComponent<GameObjectEntity>().Entity;

            EntityManager.AddComponentData(entity, new PlayerComponent());
            EntityManager.AddComponentData(entity, new HealthComponent { Value = _settings.constants.healthPlayer });
            EntityManager.AddComponentData(entity, new InputComponent());

            SpawnList.Add(spawn);
        }
    }
}