using System.Collections.Generic;
using Components;
using Installers;
using Signals;
using UniRx;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Systems
{
    [DisableAutoCreation]
    public class PlayerSpawnSystem : ComponentSystem, IPrioritySystem
    {
        public int Priority { get; }
        public readonly ReactiveDictionary<Entity, PlayerFacade> SpawnDictionary = new ReactiveDictionary<Entity, PlayerFacade>();

        private readonly SignalBus _signalBus;
        private readonly PlayerFacade.Pool _pool;
        private readonly GameSettings _settings;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly List<PlayerFacade> _destroysList = new List<PlayerFacade>();
        private ComponentGroup _group;

        public PlayerSpawnSystem(SignalBus signalBus, int priority, PlayerFacade.Pool pool, GameSettings settings)
        {
            _signalBus = signalBus;
            _pool = pool;
            _settings = settings;
            Priority = priority;
        }

        protected override void OnCreateManager()
        {
            _group = GetComponentGroup(
                ComponentType.ReadOnly<DestroyEntityComponent>(),
                ComponentType.ReadOnly<PlayerComponent>());

            _signalBus.GetStream<SignalUiLayerWantsAddPlayer>()
                .Subscribe(x => {Add(x.Count);})
                .AddTo(_disposables);
        }

        protected override void OnDestroyManager()
        {
            _disposables?.Dispose();
        }

        protected override void OnUpdate()
        {
            //this code does not work every frame
            Entities.With(_group).ForEach(
                (entity) =>
                {
                    if (SpawnDictionary.ContainsKey(entity))
                    {
                        _destroysList.Add(SpawnDictionary[entity]);
                        SpawnDictionary.Remove(entity);
                    }
                    PostUpdateCommands.DestroyEntity(entity);
                });

            foreach (var entity in _destroysList)
            {
                _pool.Despawn(entity);
            }

            _destroysList.Clear();
        }

        private void Add(int count)
        {
            for (var i = 0; i < count; i++)
            {
                var spawnPosition = new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));
                Spawn(spawnPosition, quaternion.identity);
            }
        }

        private void Spawn(Vector3 position, quaternion rotation)
        {
            var spawn = _pool.Spawn(position);
            var entity = spawn.gameObject.GetComponent<GameObjectEntity>().Entity;
            EntityManager.AddComponentData(entity, new PlayerComponent());
            EntityManager.AddComponentData(entity, new HealthComponent { Value = _settings.constants.healthPlayer });
            EntityManager.AddComponentData(entity, new InputComponent());

            SpawnDictionary.Add(entity, spawn);
        }
    }
}