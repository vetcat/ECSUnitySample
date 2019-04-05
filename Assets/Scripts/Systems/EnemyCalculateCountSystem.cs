using Components;
using Interfaces;
using Signals;
using Unity.Entities;
using UnityEngine;
using Zenject;

namespace Systems
{
    [DisableAutoCreation]
    public class EnemyCalculateCountSystem : ComponentSystem, IPrioritySystem
    {
        private readonly SignalBus _signalBus;
        private ComponentGroup _enemyGroup;
        private int _enemyCount;
        private int _enemyCountOldValue;

        public EnemyCalculateCountSystem(int priority, SignalBus signalBus)
        {
            _signalBus = signalBus;
            Priority = priority;
        }

        public int Priority { get; }

        protected override void OnCreateManager()
        {
            _enemyGroup = GetComponentGroup(
                ComponentType.ReadOnly<Transform>(),
                ComponentType.ReadOnly<EnemyComponent>(),
                ComponentType.Exclude<DestroyEntityComponent>());
        }

        protected override void OnUpdate()
        {
            _enemyCount = Entities.With(_enemyGroup).ToComponentGroup().CalculateLength();

            if (_enemyCount != _enemyCountOldValue)
            {
                _signalBus.Fire(new SignalEcsLayerEnemyCountUpdate(_enemyCount));
                _enemyCountOldValue = _enemyCount;
            }
        }
    }
}