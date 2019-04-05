using System.Collections.Generic;
using Components;
using Interfaces;
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
    public class EnemySpawnSystem : ComponentSystem, IPrioritySystem
    {
        public int Priority { get; }

        private readonly SignalBus _signalBus;
        private readonly EnemyFacade.Pool _enemyPool;
        private EntityManager _entityManager;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private readonly Dictionary<Entity, EnemyFacade> _enemySpawns = new Dictionary<Entity, EnemyFacade>();
        private readonly List<EnemyFacade> _enemyDestroysList = new List<EnemyFacade>();
        private ComponentGroup _group;

        public EnemySpawnSystem(SignalBus signalBus, int priority, EnemyFacade.Pool enemyPool)
        {
            _signalBus = signalBus;
            _enemyPool = enemyPool;
            Priority = priority;
        }

        protected override void OnCreateManager()
        {
            _entityManager = World.Active.GetExistingManager<EntityManager>();

            _group = GetComponentGroup(
                ComponentType.ReadOnly<DestroyEntityComponent>(),
                ComponentType.ReadOnly<EnemyComponent>());

            _signalBus.GetStream<SignalUiLayerWantsAddEnemy>()
                .Subscribe(x => {AddEnemy(x.Count);})
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
                    if (_enemySpawns.ContainsKey(entity))
                    {
                        _enemyDestroysList.Add(_enemySpawns[entity]);
                        _enemySpawns.Remove(entity);
                    }
                    PostUpdateCommands.DestroyEntity(entity);
                });

            foreach (var entity in _enemyDestroysList)
            {
                _enemyPool.Despawn(entity);
            }

            _enemyDestroysList.Clear();
        }

        private void AddEnemy(int count)
        {
            for (var i = 0; i < count; i++)
            {
                var spawnPosition = new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f));
                SpawnEnemy(spawnPosition, quaternion.identity);
            }
        }

        private void SpawnEnemy(Vector3 position, quaternion rotation)
        {
            var enemy = _enemyPool.Spawn(position);
            var entity = enemy.gameObject.GetComponent<GameObjectEntity>().Entity;
            _entityManager.AddComponentData(entity, new EnemyComponent());
            _entityManager.AddComponentData(entity, new HealthComponent { Value = 50 });

            _enemySpawns.Add(entity, enemy);
        }
    }
}