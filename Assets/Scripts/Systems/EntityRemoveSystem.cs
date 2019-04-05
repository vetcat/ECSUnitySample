using Components;
using Interfaces;
using Signals;
using UniRx;
using Unity.Entities;
using UnityEngine;
using Zenject;

namespace Systems
{
    [DisableAutoCreation]
    public class EntityRemoveSystem : ComponentSystem, IPrioritySystem
    {
        public int Priority { get; }

        private readonly SignalBus _signalBus;
        private EntityManager _entityManager;
        private readonly CompositeDisposable _disposables = new CompositeDisposable();
        private ComponentGroup _enemyGroup;

        public EntityRemoveSystem(SignalBus signalBus, int priority)
        {
            _signalBus = signalBus;
            Priority = priority;
        }

        protected override void OnCreateManager()
        {
            _entityManager = World.Active.GetExistingManager<EntityManager>();

            _enemyGroup = GetComponentGroup(
                ComponentType.ReadOnly<Transform>(),
                ComponentType.ReadOnly<EnemyComponent>(),
                ComponentType.Exclude<DestroyEntityComponent>());

            _signalBus.GetStream<SignalUiLayerWantsRemoveEnemy>()
                .Subscribe(RemoveEnemy)
                .AddTo(_disposables);
        }

        protected override void OnDestroyManager()
        {
            _disposables?.Dispose();
        }

        protected override void OnUpdate()
        {
        }

        private void RemoveEnemy(SignalUiLayerWantsRemoveEnemy data)
        {
            var enemies = _enemyGroup.GetEntityArray();
            if (enemies.Length == 0)
            {
                return;
            }

            _entityManager.AddComponentData(enemies[0], new DestroyEntityComponent());
        }
    }
}