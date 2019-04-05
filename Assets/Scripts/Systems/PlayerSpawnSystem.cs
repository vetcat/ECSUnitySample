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

        private readonly SignalBus _signalBus;
        private readonly PlayerFacade.Pool _enemyPool;
        private readonly GameSettings _settings;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly Dictionary<Entity, PlayerFacade> _spawnDictionary = new Dictionary<Entity, PlayerFacade>();
        private readonly List<PlayerFacade> _destroysList = new List<PlayerFacade>();
        private ComponentGroup _group;

        public PlayerSpawnSystem(SignalBus signalBus, int priority, PlayerFacade.Pool enemyPool, GameSettings settings)
        {
            _signalBus = signalBus;
            _enemyPool = enemyPool;
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
                    if (_spawnDictionary.ContainsKey(entity))
                    {
                        _destroysList.Add(_spawnDictionary[entity]);
                        _spawnDictionary.Remove(entity);
                    }
                    PostUpdateCommands.DestroyEntity(entity);
                });

            foreach (var entity in _destroysList)
            {
                _enemyPool.Despawn(entity);
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
            var enemy = _enemyPool.Spawn(position);
            var entity = enemy.gameObject.GetComponent<GameObjectEntity>().Entity;
            EntityManager.AddComponentData(entity, new PlayerComponent());
            EntityManager.AddComponentData(entity, new HealthComponent { Value = _settings.constants.healthPlayer });
            EntityManager.AddComponentData(entity, new InputComponent());

            _spawnDictionary.Add(entity, enemy);
        }
    }
}